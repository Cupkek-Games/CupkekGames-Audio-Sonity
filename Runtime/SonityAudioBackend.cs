using Sonity;
using UnityEngine;
using CupkekGames.Audio;

namespace CupkekGames.Audio.Sonity
{
    public class SonityAudioBackend : MonoBehaviour, IAudioBackend
    {
        [SerializeField] private SoundManager _soundManager;

        private SoundEvent GetSoundEvent(IAudioCue cue)
        {
            if (cue is SonityAudioCueSO sonityCue)
                return sonityCue.SoundEvent;
            return null;
        }

        public void PlayMusic(IAudioCue cue, bool stopAllOtherMusic = true, bool allowFadeOut = true)
        {
            var soundEvent = GetSoundEvent(cue);
            if (soundEvent != null)
                _soundManager.MusicPlay(soundEvent, stopAllOtherMusic, allowFadeOut);
        }

        public void StopAllMusic(bool allowFadeOut = true)
        {
            _soundManager.MusicStopAll(allowFadeOut);
        }

        public void PlayUI(IAudioCue cue)
        {
            var soundEvent = GetSoundEvent(cue);
            if (soundEvent != null)
                _soundManager.UIPlay(soundEvent);
        }

        public void StopUI(IAudioCue cue, bool allowFadeOut = true)
        {
            var soundEvent = GetSoundEvent(cue);
            if (soundEvent != null)
                _soundManager.UIStop(soundEvent, allowFadeOut);
        }
    }
}
