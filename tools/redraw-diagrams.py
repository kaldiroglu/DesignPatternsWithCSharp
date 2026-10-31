#!/usr/bin/env python3
"""
Redraws bent is-a lines in the rendered class diagrams.

The rule for a class diagram: the parent sits above and centered over its children, the
children sit side by side on one row, and every line is straight. The `.puml` hints set the
positions. Graphviz still bends an is-a line now and then, and no hint removes that, so this
step replaces each bent line with a straight one in the `.svg` and renders the `.png` again
from that `.svg`. Only lines change; no box moves.

Run it after `plantuml -tsvg -tpng`, because PlantUML overwrites both images. It finds the
files itself: it reads every `.svg` under the given paths and leaves one without a bent is-a
line untouched, so a second run changes nothing. A `.png` beside the `.svg` keeps its width.

A line that would cross a box stays bent and is reported, and the exit code is then 1.

Needs Python 3 and rsvg-convert (brew install librsvg).

Run it with:  python3 tools/redraw-diagrams.py [--check] [path ...]
              --check reports the bent lines and writes nothing
"""

import math
import re
import struct
import subprocess
import sys
from pathlib import Path

REPO = Path(__file__).resolve().parent.parent
DEFAULT_ROOTS = [REPO]
SKIPPED_FOLDERS = {"bin", "obj"}

STRAIGHT_TOLERANCE = 3.0  # px a point may sit off the line and still count as straight
SAME_COLUMN = 4.0         # px between box centers that count as one above the other

LINK = re.compile(
    r'<g class="link" data-entity-1="(ent\d+)" data-entity-2="(ent\d+)"'
    r' data-link-type="extension".*?</g>',
    re.S,
)


def read_boxes(svg):
    """Maps each entity id to its name and rectangle."""
    boxes = {}
    for match in re.finditer(r'<g class="entity" ([^>]*)>(.*?)</g>', svg, re.S):
        attributes = dict(re.findall(r'([\w-]+)="([^"]*)"', match[1]))
        rect = re.search(r"<rect ([^>]*)>", match[2])
        if not rect or "id" not in attributes:
            continue
        size = dict(re.findall(r'\b(height|width|x|y)="([\d.]+)"', rect[1]))
        boxes[attributes["id"]] = {
            "name": attributes.get("data-qualified-name", ""),
            "x": float(size["x"]),
            "y": float(size["y"]),
            "w": float(size["width"]),
            "h": float(size["height"]),
        }
    return boxes


def is_straight(points):
    start, end = points[0], points[-1]
    length = math.dist(start, end) or 1
    return all(
        abs((end[0] - start[0]) * (p[1] - start[1]) - (end[1] - start[1]) * (p[0] - start[0]))
        / length <= STRAIGHT_TOLERANCE
        for p in points
    )


def distance_to_box(point, box):
    dx = max(box["x"] - point[0], 0, point[0] - box["x"] - box["w"])
    dy = max(box["y"] - point[1], 0, point[1] - box["y"] - box["h"])
    return math.hypot(dx, dy)


def crosses_a_box(start, end, boxes, skip):
    """True when the segment runs through a visible box other than the two it joins."""
    for key, box in boxes.items():
        if key in skip or "Spacer" in box["name"]:
            continue
        for step in range(1, 100):
            x = start[0] + (end[0] - start[0]) * step / 100
            y = start[1] + (end[1] - start[1]) * step / 100
            if box["x"] < x < box["x"] + box["w"] and box["y"] < y < box["y"] + box["h"]:
                return True
    return False


def redraw(svg):
    """Returns the new SVG text, the number of lines redrawn and the number left bent."""
    boxes = read_boxes(svg)
    counts = {"redrawn": 0, "left": 0}

    def redraw_link(match):
        group = match[0]
        path = re.search(r' d="([^"]*)"', group)
        head = re.search(r'<polygon[^>]*points="([^"]+)"', group)
        if not path or not head or match[1] not in boxes or match[2] not in boxes:
            return group
        points = [(float(x), float(y)) for x, y in re.findall(r"(-?[\d.]+),(-?[\d.]+)", path[1])]
        if is_straight(points):
            return group

        numbers = [float(value) for value in head[1].split(",")]
        tip, left, right = tuple(numbers[0:2]), tuple(numbers[2:4]), tuple(numbers[4:6])
        first, second = match[1], match[2]
        if distance_to_box(tip, boxes[first]) <= distance_to_box(tip, boxes[second]):
            parent, child = first, second
        else:
            parent, child = second, first
        parent_box, child_box = boxes[parent], boxes[child]

        head_length = math.dist(tip, ((left[0] + right[0]) / 2, (left[1] + right[1]) / 2))
        head_half_width = math.dist(left, right) / 2
        parent_center = parent_box["x"] + parent_box["w"] / 2
        child_center = child_box["x"] + child_box["w"] / 2
        if abs(parent_center - child_center) < SAME_COLUMN:
            tip = (parent_center, parent_box["y"] + parent_box["h"])
            far_end = (parent_center, child_box["y"])
        else:
            far_end = max(points[0], points[-1], key=lambda p: math.dist(p, tip))

        if crosses_a_box(far_end, tip, boxes, {parent, child}):
            counts["left"] += 1
            return group

        length = math.dist(tip, far_end)
        ux, uy = (tip[0] - far_end[0]) / length, (tip[1] - far_end[1]) / length
        base = (tip[0] - head_length * ux, tip[1] - head_length * uy)
        corner_a = (base[0] - head_half_width * uy, base[1] + head_half_width * ux)
        corner_b = (base[0] + head_half_width * uy, base[1] - head_half_width * ux)
        new_head = ",".join(f"{value:.4f}" for value in (*tip, *corner_a, *corner_b, *tip))
        new_path = f"M{far_end[0]:.4f},{far_end[1]:.4f} L{base[0]:.4f},{base[1]:.4f}"
        counts["redrawn"] += 1
        return group.replace(head[1], new_head).replace(path[1], new_path)

    return LINK.sub(redraw_link, svg), counts["redrawn"], counts["left"]


def png_width(path):
    with open(path, "rb") as image:
        return struct.unpack(">I", image.read(24)[16:20])[0]


def render_png(svg_path):
    png_path = svg_path.with_suffix(".png")
    if not png_path.exists():
        return
    subprocess.run(
        ["rsvg-convert", "-b", "white", "-w", str(png_width(png_path)),
         "-o", str(png_path), str(svg_path)],
        check=True,
    )


def svg_files(paths):
    for path in paths:
        files = [path] if path.is_file() else sorted(path.rglob("*.svg"))
        for file in files:
            if not SKIPPED_FOLDERS & set(file.parts):
                yield file


def main(arguments):
    check_only = "--check" in arguments
    paths = [Path(a).resolve() for a in arguments if a != "--check"] or DEFAULT_ROOTS
    changed = 0
    left_bent = 0
    for svg_path in svg_files(paths):
        svg = svg_path.read_text(encoding="utf-8", errors="replace")
        new_svg, redrawn, left = redraw(svg)
        left_bent += left
        if left:
            print(f"left bent ({left}): {svg_path}")
        if not redrawn:
            continue
        changed += 1
        print(f"{'bent' if check_only else 'redrawn'} ({redrawn}): {svg_path}")
        if not check_only:
            svg_path.write_text(new_svg, encoding="utf-8")
            render_png(svg_path)
    print(f"{changed} diagram(s) {'with bent lines' if check_only else 'redrawn'}, "
          f"{left_bent} line(s) left bent")
    return 1 if left_bent or (check_only and changed) else 0


if __name__ == "__main__":
    sys.exit(main(sys.argv[1:]))
