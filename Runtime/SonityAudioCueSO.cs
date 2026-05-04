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
    }
}
