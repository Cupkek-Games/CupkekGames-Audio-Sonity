using Sonity;
using UnityEngine;
using CupkekGames.Audio;

namespace CupkekGames.Audio.Sonity
{
    [CreateAssetMenu(fileName = "SonityAudioCue", menuName = "CupkekGames/Audio/Sonity/Audio Cue")]
    public class SonityAudioCueSO : ScriptableObject, IAudioCue
    {
        [SerializeField] private SoundEvent _soundEvent;
        public SoundEvent SoundEvent => _soundEvent;

        public void Play(Transform transform)
        {
            _soundEvent?.Play(transform);
        }

        public void Stop(Transform transform)
        {
            _soundEvent?.Stop(transform);
        }

        public void PlayUI()
        {
            _soundEvent?.UIPlay();
        }

        public void StopUI()
        {
            _soundEvent?.UIStop();
        }
    }
}
