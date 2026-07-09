# Structural Design Patterns (C# — Business)

Each subdirectory is a standalone .NET console project. Run any pattern with:

```bash
cd <Pattern>
dotnet run
```

Both the problem (anti-pattern) and the solution (patterned) demos are printed in sequence.

| Pattern   | Scenario |
|-----------|----------|
| Adapter   | Wiring a checkout service to a legacy payment gateway with two-step authorize/capture |
| Bridge    | Notifications × delivery channels (renderer in the abstraction, not the channel) |
| Composite | Org hierarchy with iterative DFS + identity-based cycle protection |
| Decorator | HTTP client pipeline (Auth → Retry → Cache → Logging — order matters) |
| Facade    | Checkout orchestration with compensation on payment/shipping failure |
| Flyweight | Market-data feed handler interning Instrument metadata |
| Proxy     | `CustomerProfileService` with Authorization OUTSIDE Caching |

Each `Problem.md` calls out the trap an experienced developer should avoid.
