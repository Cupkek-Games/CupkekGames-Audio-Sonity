# CupkekGames Audio — Sonity Bridge

Concrete backend for [CupkekGames.Audio](https://github.com/Cupkek-Games/CupkekGames-Audio) on top of the [Sonity](https://assetstore.unity.com/packages/audio/music/sonity-audio-system-189491) audio asset, with TimeSystem time-scaling integration.

## What's inside

**Runtime** (`CupkekGames.Audio.Sonity.asmdef`)

- `SonityAudioBackend` — `IAudioBackend` impl; routes Audio API calls to Sonity at the scene root
- `SonityAudioCueSO` — `IAudioCue` ScriptableObject backed by Sonity's `SoundEvent`
- `SonitySFXPlayerSO` — fire-and-forget SFX player wired into `CupkekGames.TimeSystem`
- `TimeBundleSonityInstaller` — installs Sonity's `ITimeScaler` into a `TimeBundle`
- `TimeScaleSonity` — `ITimeScaler` impl that drives Sonity playback speed from a `TimeContext`

## Dependencies

- `com.cupkekgames.audio` (UPM)
- `com.cupkekgames.timesystem` (UPM)
- Sonity Asset Store package (project-level — bring your own)

## HM-internal dep

Bridge currently references `CupkekGames.VFX` which is not yet on the registry. Consumers outside HeroManager need to either fork that asmdef or wait for its extraction.
