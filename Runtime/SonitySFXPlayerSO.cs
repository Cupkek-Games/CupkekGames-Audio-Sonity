using Sonity;
using UnityEngine;
using CupkekGames.TimeSystem;
using CupkekGames.Audio;

namespace CupkekGames.Audio.Sonity
{
    [CreateAssetMenu(fileName = "SonitySFXPlayer", menuName = "CupkekGames/Integration/Sonity/SFX Player")]
    public class SonitySFXPlayerSO : SFXPlayerSO
    {
        [SerializeField] private SoundEvent _soundEvent;

        public override void Play(Transform transform)
        {
            if (_soundEvent != null)
                _soundEvent.Play(transform);
        }

        public override void RegisterTimeScale(TimeBundle timeBundle, Transform owner)
        {
            if (_soundEvent == null || timeBundle == null) return;

            var sonityScaler = timeBundle.GetScaler<TimeScaleSonity>();
            if (sonityScaler != null)
            {
                sonityScaler.Add(_soundEvent, owner,
                    new SoundParameterPitchRatio(1f, UpdateMode.Continuous));
            }
        }
    }
}
