# Facade — Problem

A home theater has many subsystems: amplifier, DVD player, projector, screen, lights. To "watch a movie" the client must dim the lights, lower the screen, turn on the projector and amp, then start the DVD.

## Without the pattern

The client knows every subsystem and the correct sequence. Repeated everywhere a movie is started or stopped.

See `Problem/`.

## With the Facade pattern

`HomeTheaterFacade.WatchMovie(...)` and `EndMovie()` encapsulate the orchestration. The client makes one call.

See `Solution/`.
