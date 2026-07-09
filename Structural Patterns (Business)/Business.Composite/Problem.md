# Composite — Org Hierarchy Rollup

An HR/finance app needs to answer questions about the company's org chart:

- Total annual salary cost for any subtree.
- Total headcount under a node.
- Find an employee by id from any starting point.

The chart is recursive: a department contains employees and other departments.

## Without the pattern

Every walker writes its own type-test ladder, duplicates the recursion, and conflates structure traversal with the question being asked.

See `Problem/`.

## With the Composite pattern

`IOrgUnit` with `Name`, `Headcount`, `TotalSalaryCost`, `FindEmployee(id)`. `Department.TotalSalaryCost` recurses uniformly.

What experienced developers must still get right:

- **Cycle protection** — a department list of `IOrgUnit` accidentally containing one of its ancestors will infinite-recurse. Iterative DFS with identity-based visited set avoids this.
- **Find performance** — `FindEmployee` is a linear walk; for huge trees consider an index.

See `Solution/`.
