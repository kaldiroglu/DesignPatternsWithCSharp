# Flyweight — Market-Data Quotes

A trading-floor app processes hundreds of millions of `Quote` events per day. Each quote carries:

- **Per-event** state: bid, ask, last, volume, timestamp.
- **Instrument metadata**: symbol, exchange, currency, ISIN, sector, full company name, tick size, lot size, trading hours.

Metadata for `AAPL` is the *same* for every `AAPL` quote.

## Without the pattern

`Quote` carries the full instrument metadata: every event allocates new strings. GC saturates; cache misses on every tick.

See `Problem/`.

## With the Flyweight pattern

- **Intrinsic** (immutable, shareable): `Instrument` — symbol, exchange, currency, ISIN, sector, name, tick size, lot size, trading hours.
- **Extrinsic** (per-event): `Quote` — bid, ask, last, volume, timestamp + reference to interned `Instrument`.

`InstrumentRegistry` interns `Instrument` instances by composite key. Every `Quote` for `AAPL` shares the same `Instrument`.

Hard parts to respect:

- **Immutability of intrinsic state** is a contract.
- **Identity equality** — interning means `==` works for instrument comparison.
- **Bounded growth** — the registry must not be a memory leak.

See `Solution/`.
