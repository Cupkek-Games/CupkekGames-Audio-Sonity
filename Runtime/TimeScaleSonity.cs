using System.Collections.Generic;
using UnityEngine;
using Sonity;
using CupkekGames.TimeSystem;

namespace CupkekGames.Audio.Sonity
{
    /// <summary>
    /// Manages Sonity SoundEvent components and controls their pause state and pitch according to a TimeContext.
    /// When time scale is 0, sounds are paused. When time scale is > 0, sounds are unpaused and pitch is adjusted.
    /// </summary>
    public class TimeScaleSonity : ITimeScaler
    {
        public TimeContext Context;

        // Store SoundEvent and Transform pairs with their pitch parameters
        [SerializeField] private Dictionary<SoundEvent, Dictionary<Transform, SoundParameterPitchRatio>> _collection = new Dictionary<SoundEvent, Dictionary<Transform, SoundParameterPitchRatio>>();

        public TimeScaleSonity(TimeContext context)
        {
            Context = context ?? TimeManager.Instance?.Global;
            if (Context != null)
                Context.OnTimeScaleChanged += OnTimeScaleChanged;
        }

        public void Add(SoundEvent soundEvent, Transform owner, SoundParameterPitchRatio soundParameterPitchRatio)
        {
            if (soundEvent != null && owner != null)
            {
                // Initialize Dictionary if it doesn't exist
                if (!_collection.ContainsKey(soundEvent))
                {
                    _collection[soundEvent] = new Dictionary<Transform, SoundParameterPitchRatio>();
                }

                // Add the owner if not already present
                if (!_collection[soundEvent].ContainsKey(owner))
                {
                    _collection[soundEvent][owner] = soundParameterPitchRatio;

                    // Apply current time scale
                    ApplyTimeScale(soundEvent, owner, soundParameterPitchRatio);
                }
            }
        }

        public void Remove(SoundEvent soundEvent, Transform owner)
        {
            if (soundEvent != null && owner != null && _collection.ContainsKey(soundEvent))
            {
                if (_collection[soundEvent].ContainsKey(owner))
                {
                    // Reset pitch to 1.0 before removing
                    var pitchParam = _collection[soundEvent][owner];
                    if (pitchParam != null)
                    {
                        pitchParam.PitchRatio = 1f;
                    }

                    // Unpause before removing to ensure sound is in a good state
                    soundEvent.Unpause(owner);
                    _collection[soundEvent].Remove(owner);
                }
            }
        }

        public void Clear()
        {
            foreach (var soundEvent in _collection.Keys)
            {
                foreach (var owner in _collection[soundEvent].Keys)
                {
                    if (soundEvent != null && owner != null)
                    {
                        // Reset pitch to 1.0
                        var pitchParam = _collection[soundEvent][owner];
                        if (pitchParam != null)
                        {
                            pitchParam.PitchRatio = 1f;
                        }

                        // Unpause all sounds before clearing
                        soundEvent.Unpause(owner);
                    }
                }
            }
            _collection.Clear();
        }

        public void ClearInactive()
        {
            Dictionary<SoundEvent, Dictionary<Transform, SoundParameterPitchRatio>> newCollection = new Dictionary<SoundEvent, Dictionary<Transform, SoundParameterPitchRatio>>();

            foreach (var soundEvent in _collection.Keys)
            {
                if (soundEvent != null)
                {
                    newCollection[soundEvent] = new Dictionary<Transform, SoundParameterPitchRatio>();

                    foreach (var owner in _collection[soundEvent].Keys)
                    {
                        if (owner != null && owner.gameObject.activeInHierarchy)
                        {
                            newCollection[soundEvent][owner] = _collection[soundEvent][owner];
                        }
                        else if (owner != null)
                        {
                            // Reset pitch and unpause inactive objects before removing them
                            var pitchParam = _collection[soundEvent][owner];
                            if (pitchParam != null)
                            {
                                pitchParam.PitchRatio = 1f;
                            }
                            soundEvent.Unpause(owner);
                        }
                    }
                }
            }

            _collection = newCollection;
        }

        private void ApplyTimeScale(SoundEvent soundEvent, Transform owner, SoundParameterPitchRatio soundParameterPitchRatio)
        {
            if (soundEvent == null || owner == null || !_collection.ContainsKey(soundEvent) || !_collection[soundEvent].ContainsKey(owner))
                return;

            var timeScale = Context.TimeScale;

            if (timeScale <= 0f)
            {
                // Pause the sound when time scale is 0 or negative
                soundEvent.Pause(owner);
            }
            else
            {
                // Unpause when time scale is positive
                soundEvent.Unpause(owner);
            }

            // Apply pitch scaling if pitch parameter is provided
            if (soundParameterPitchRatio != null)
            {
                soundParameterPitchRatio.PitchRatio = timeScale;
            }
        }

        private void OnTimeScaleChanged(float timeScale)
        {
            foreach (var soundEvent in _collection.Keys)
            {
                foreach (var owner in _collection[soundEvent].Keys)
                {
                    var pitchParam = _collection[soundEvent][owner];
                    ApplyTimeScale(soundEvent, owner, pitchParam);
                }
            }
        }

        public void Dispose()
        {
            if (Context != null)
                Context.OnTimeScaleChanged -= OnTimeScaleChanged;
            Clear();
        }
    }
}