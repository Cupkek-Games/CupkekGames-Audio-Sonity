using Sonity;
using UnityEngine;
using CupkekGames.Audio;

namespace CupkekGames.Audio.Sonity
{
    /// <summary>
    /// Sonity-backed implementation of <see cref="IAudioBackend"/>. Routes channel strings
    /// to Sonity's MusicPlay / UIPlay / SoundEvent.Play(transform) APIs.
    /// </summary>
    public class SonityAudioBackend : MonoBehaviour, IAudioBackend
    {
        [SerializeField] private SoundManager _soundManager;

        private static SoundEvent GetSoundEvent(IAudioCue cue)
        {
            return cue is SonityAudioCueSO sonityCue ? sonityCue.SoundEvent : null;
        }

        public void Play(string channel, IAudioCue cue, IAudioPlaybackContext context = null)
        {
            SoundEvent soundEvent = GetSoundEvent(cue);
            if (soundEvent == null) return;

            AudioPlaybackContext ctx = context as AudioPlaybackContext;

            switch (channel)
            {
                case AudioChannels.Music:
                {
                    bool stopAllOthers = ctx?.StopAllOthers ?? true;
                    bool allowFadeOut = ctx?.AllowFadeOut ?? true;
                    _soundManager.MusicPlay(soundEvent, stopAllOthers, allowFadeOut);
                    break;
                }
                case AudioChannels.SFX when ctx?.Source != null:
                    soundEvent.Play(ctx.Source);
                    break;
                // Ambient / UI / SFX2D / unknown 2D channels — non-positional via Sonity's UI route.
                default:
                    _soundManager.UIPlay(soundEvent);
                    break;
            }
        }

        public void Stop(string channel, IAudioCue cue, IAudioPlaybackContext context = null)
        {
            SoundEvent soundEvent = GetSoundEvent(cue);
            if (soundEvent == null) return;

            AudioPlaybackContext ctx = context as AudioPlaybackContext;

            switch (channel)
            {
                case AudioChannels.SFX when ctx?.Source != null:
                    soundEvent.Stop(ctx.Source);
                    break;
                default:
                {
                    bool allowFadeOut = ctx?.AllowFadeOut ?? true;
                    _soundManager.UIStop(soundEvent, allowFadeOut);
                    break;
                }
            }
        }

        public void StopAll(string channel, IAudioPlaybackContext context = null)
        {
            AudioPlaybackContext ctx = context as AudioPlaybackContext;
            bool allowFadeOut = ctx?.AllowFadeOut ?? true;

            if (channel == AudioChannels.Music)
            {
                _soundManager.MusicStopAll(allowFadeOut);
            }
            // Sonity has no global UI-stop; per-cue Stop is the route. No-op for other channels.
        }
    }
}
