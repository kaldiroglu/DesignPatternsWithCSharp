# Decorator — Problem

A coffee shop offers a base coffee plus optional add-ons (milk, sugar, vanilla). Each add-on changes description and price.

## Without the pattern

A subclass per combination — `CoffeeWithMilk`, `CoffeeWithSugar`, `CoffeeWithMilkAndSugar`, etc. *2^N* classes.

See `Problem/`.

## With the Decorator pattern

`ICoffee` interface, one base `SimpleCoffee`, decorators that wrap and stack at runtime.

See `Solution/`.
