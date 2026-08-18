#!/usr/bin/env bash
#
# Renders every .puml under src/Bridge to PNG and SVG, beside its source.
#
# The diagrams live in a uml/ folder next to the code they describe, which is the layout
# the Java repository uses. Nothing in the build depends on them: the .csproj globs *.cs
# only, so .puml, .png and .svg are inert as far as MSBuild is concerned.
#
# Needs PlantUML on the PATH (brew install plantuml).

set -euo pipefail

here="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

if ! command -v plantuml > /dev/null; then
    echo "plantuml not found on PATH — brew install plantuml" >&2
    exit 1
fi

count=0
while IFS= read -r -d '' file; do
    plantuml -tpng "$file"
    plantuml -tsvg "$file"
    count=$((count + 1))
    echo "  $(basename "$file")"
done < <(find "$here/src/Bridge" -name '*.puml' -print0 | sort -z)

echo "$count diagram(s) rendered"
