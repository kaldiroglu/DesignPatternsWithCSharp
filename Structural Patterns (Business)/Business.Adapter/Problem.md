# Adapter — Payment Gateway Integration

## Scenario

`CheckoutService` charges customers through an `IPaymentProcessor` interface:

```csharp
PaymentResult result = paymentProcessor.Charge(PaymentRequest req);
```

`PaymentRequest` carries a `Money` value type (currency + decimal amount), an idempotency key, customer reference, and metadata. `PaymentResult` is a discriminated-union-style sealed record returning either a successful authorization id or a typed failure.

We just signed a deal with **Acme Pay**, whose SDK is, charitably, "of its time":

- Amounts are `long` minor units.
- It exposes a **two-step** flow: `Authorize(...)` returns an auth token; `Capture(authToken, ...)` settles the funds. Our codebase wants a single call.
- It throws `AcmeGatewayException` with integer error codes — no enum, no typed reasons.
- Idempotency is per-method, with a different field name (`requestUid`) and only on `Authorize`.

We don't own the SDK and won't change `CheckoutService`.

## Without the pattern

`CheckoutService` learns about `AcmeGatewayClient`, the cents conversion, the auth/capture orchestration, and the error-code translation. Tomorrow when we add Stripe or Adyen, every one of those concerns leaks into checkout again.

See `Problem/`.

## With the Adapter pattern

`AcmePayAdapter : IPaymentProcessor` owns:

- **Amount conversion** — `decimal` ↔ minor units, currency-aware (JPY has no minor units).
- **Two-step orchestration** — single `Charge()` calls `Authorize` then `Capture`, releasing the auth on capture failure.
- **Error translation** — `AcmeGatewayException(code)` becomes `PaymentResult.Failure(reason)`.
- **Idempotency mapping** — domain idempotency key → `requestUid`.

`CheckoutService` stays pure. New gateway = new adapter.

See `Solution/`.
