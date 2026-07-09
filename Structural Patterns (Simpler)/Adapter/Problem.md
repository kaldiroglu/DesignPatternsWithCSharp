# Adapter — Problem

`AudioPlayer` plays MP3 natively. We also need to play VLC and MP4 files, but those formats come from third-party libraries with **different interfaces** than the one our client expects.

## Without the pattern

`AudioPlayer.Play()` becomes a growing `if/else` chain that hard-codes every new format. Every new format means cracking open `AudioPlayer` again — open/closed violated.

See `Problem/`.

## With the Adapter pattern

We introduce `IAdvancedMediaPlayer` for third-party players and a `MediaAdapter` that translates calls on the client-facing `IMediaPlayer` interface into calls on `IAdvancedMediaPlayer`. `AudioPlayer` only knows `IMediaPlayer` and delegates unknown formats to a `MediaAdapter`. New format = new adapter, no edits to existing code.

See `Solution/`.
