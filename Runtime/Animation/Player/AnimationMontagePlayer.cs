using System;
using System.Collections.Generic;
using RinaGameplay.Ability;
using RinaGameplay.Animation.Montage;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace RinaGameplay.Animation.Player {
    
    public interface IAnimationMontagePlayer {
        
        event Action<AnimationMontageNotify> OnTriggerNotify;
        
        event Action<AnimationMontageNotifyState> OnStateStart;
        
        event Action<AnimationMontageNotifyState> OnStateEnd;
        
        event Action OnMontageComplete;
        
        void PlayMontage (IAnimationMontage montage, AnimationMontageSection section = null, IActiveGameplayAbility ability = null);
        
        void StopMontage (float blendOutTime = 0.0f);
        
    }
    
    [RequireComponent(typeof(Animator))]
    public class AnimationMontagePlayer : SerializedMonoBehaviour, IAnimationMontagePlayer {
        
        private Animator _animator;
        
        private PlayableGraph _graph;

        private AnimationMixerPlayable _mixerPlayable;
        
        private IAnimationMontage _currentMontage;

        private float _currentTime;

        private bool _isPlaying = false;
        
        private List<AnimationMontageNotify> _pendingNotifies = new List<AnimationMontageNotify>();
        
        private List<AnimationMontageNotifyState> _activeStates = new List<AnimationMontageNotifyState>();
        
        public event Action<AnimationMontageNotify> OnTriggerNotify;

        public event Action<AnimationMontageNotifyState> OnStateStart;
        
        public event Action<AnimationMontageNotifyState> OnStateEnd;

        public event Action OnMontageComplete;

        private void Awake() {
            _animator = gameObject.GetComponent<Animator>();
            InitPlayableGraph();
        }

        private void Start() {
            
        }

        private void Update() {
            UpdateCurrentTime();
            ProcessNotifies();
            ProcessNotifyStates();
            if (IsSectionEnd()) {
                CompleteMontage();
            }
        }

        public void PlayMontage(IAnimationMontage montage, AnimationMontageSection section = null, IActiveGameplayAbility ability = null) {
            if (montage is null || montage.Clip is null) {
                return;
            }
            
            if (_isPlaying) StopMontage();

            _currentMontage = montage;
            _isPlaying = true;

            float startTime = 0.0f;
            if (section is not null) {
                startTime = section.startTime;
            }

            _currentTime = startTime;
            
            var clipPlayable = AnimationClipPlayable.Create(_graph, _currentMontage.Clip);
            clipPlayable.SetSpeed(montage.PlayRate);
            clipPlayable.SetTime(_currentTime);
            
            _mixerPlayable.ConnectInput(0, clipPlayable, 0,1.0f);
            _graph.Play();
            
            _pendingNotifies.Clear();
            _pendingNotifies.AddRange(_currentMontage.Notifies);
            
            _activeStates.Clear();
            
        }

        public void StopMontage(float blendOutTime = 0.0f) {
            if (_isPlaying is false) return;

            foreach (var state in _activeStates) {
                OnStateStart?.Invoke(state);
            }
            _activeStates.Clear();
            _isPlaying = false;
            _currentMontage = null;

            if (_graph.IsValid()) {
                _mixerPlayable.DisconnectInput(0);
            }
        }

        private void InitPlayableGraph() {
            
        }

        private void UpdateCurrentTime() {
            if (_isPlaying is false || _currentMontage is null) return;

            _currentTime += Time.deltaTime * _currentMontage.PlayRate;
        }

        private void ProcessNotifies() {
            if (_isPlaying is false || _currentMontage is null) return;
            for (int i = _pendingNotifies.Count -1; i >= 0; i--) {
                var notify = _pendingNotifies[i];
                if (notify.TriggerTime <= _currentTime) {
                    OnTriggerNotify?.Invoke(notify);
                    _pendingNotifies.RemoveAt(i);
                }
            }
        }

        private void ProcessNotifyStates() {
            if (_isPlaying is false || _currentMontage is null) return;
            foreach (var state in _currentMontage.NotifyStates) {
                bool isActive = _activeStates.Contains(state);
                bool shouldBeActive = state.StartTime <= _currentTime && _currentTime <= state.EndTime;
                if (shouldBeActive && !isActive) {
                    _activeStates.Add(state);
                    OnStateStart?.Invoke(state);
                }
                else if (!shouldBeActive && isActive) {
                    _activeStates.Remove(state);
                    OnStateEnd?.Invoke(state);
                }
            }
        }

        private bool IsSectionEnd() {
            if (_currentMontage is null) return false;
            return _currentTime >= _currentMontage.Duration;
        }

        private void CompleteMontage() {
            OnMontageComplete?.Invoke();
            StopMontage();
        }
    }
}