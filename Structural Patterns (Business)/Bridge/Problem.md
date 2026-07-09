# Bridge — Notifications × Channels

A SaaS sends user notifications. Two dimensions vary independently:

- **What** — `OrderConfirmation`, `PasswordReset`, `MarketingCampaign`, `SecurityAlert`
- **How** — `Email`, `SMS`, `Slack`, `Push`

Each notification has its own templating; each channel has its own constraints (HTML vs. 160-char SMS vs. Slack block kit).

## Without the pattern

One class per pair: `EmailOrderConfirmation`, `SmsOrderConfirmation`, ... *N × M* classes. New channel = *N* new classes. Templates and channel logic duplicated.

See `Problem/`.

## With the Bridge pattern

- **Abstraction** — `Notification` (subclasses: `OrderConfirmation`, `PasswordReset`). Holds an `IDeliveryChannel` and renders itself for that channel.
- **Implementor** — `IDeliveryChannel` (impls: `EmailChannel`, `SmsChannel`, `SlackChannel`).

The notification owns *what is said*; the channel owns *how it is sent*. *N + M* instead of *N × M*.

Subtlety: SMS truncation belongs to the abstraction's SMS rendering, not the SMS channel — the channel doesn't know what's safe to drop.

See `Solution/`.
