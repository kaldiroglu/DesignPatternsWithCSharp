# Facade — Checkout Orchestration

Placing an order spans many subsystems: inventory, pricing, tax, payment, shipping, persistence, notifications. The order matters; some failures must **compensate** prior steps (payment fails → release reservation; shipping fails after charge → refund).

## Without the pattern

Every controller/CLI/job re-implements the seven-step dance and the rollback rules. Twenty lines turn into hundreds; subsystem changes ripple across them.

See `Problem/`.

## With the Facade pattern

`CheckoutFacade.PlaceOrder(...)` returns a typed `CheckoutResult`. The facade owns the order, the compensation, and the exception → result mapping. The controller becomes a one-liner.

A facade is **not** a god object: subsystems remain independently usable for cases that aren't the common path (admin tools, the inventory dashboard).

See `Solution/`.
