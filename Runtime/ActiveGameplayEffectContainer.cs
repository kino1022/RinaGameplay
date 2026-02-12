using System;
using System.Collections.Generic;
using System.Linq;
using RinaGameplay.Effect;
using UnityEngine;
using VContainer.Unity;

namespace RinaGameplay {

    public interface IActiveGameplayEffectContainer {
        
        IReadOnlyList<IActiveGameplayEffect> ActiveEffects { get; }

        IReadOnlyDictionary<int, IActiveGameplayEffect> HandleToEffectMap { get; }

        ActiveGameplayEffectHandle ApplyGameplayEffectSpec(GameplayEffectSpec spec);

        bool RemoveActiveGameplayEffect(ActiveGameplayEffectHandle handle);
        
    }
    
    public class ActiveGameplayEffectContainer : IActiveGameplayEffectContainer, ITickable {

        private readonly GameplayAbilitySystem _owner;

        private readonly GameplayEffectExpiredCheckService _expiredChecker;
        
        private List<IActiveGameplayEffect> _activeEffects = new List<IActiveGameplayEffect>();
        
        private Dictionary<int,IActiveGameplayEffect> _handleToEffectMap = new Dictionary<int,IActiveGameplayEffect>();
        
        public IReadOnlyList<IActiveGameplayEffect> ActiveEffects => _activeEffects;

        public ActiveGameplayEffectContainer(GameplayAbilitySystem owner) {
            _owner = owner ?? throw new ArgumentNullException(nameof(owner));
            _expiredChecker = new GameplayEffectExpiredCheckService(this);
        }

        public ActiveGameplayEffectHandle ApplyGameplayEffectSpec(IGameplayEffectSpec spec) {
            if (!spec.Definition.CanApply(_owner)) {
                return ActiveGameplayEffectHandle.InValid;
            }

            if (spec.Definition.DurationType == GameplayEffectDurationType.Instant) {
                ExecuteInstantEffect(spec);
                return ActiveGameplayEffectHandle.InValid;
            }

            if (spec.Definition.StackingType != GameplayEffectStackingType.None) {
                var existingEffect = FindStackableEffect(spec);
                if (existingEffect is not null) {
                    return HandleStacking(existingEffect, spec);
                }
            }

            IActiveGameplayEffect activeEffect = new ActiveGameplayEffect(spec);
            activeEffect.SetHandle(ActiveGameplayEffectHandle.GenerateNewHandle());
            activeEffect.SetStartTime(Time.time);
            
            spec.RecalculateModifiers(_owner);
            ApplyModifiersToTarget(activeEffect, spec);
            
            ApplyGrantedTags(spec);
            GrantAbilities(spec);
            
            _activeEffects.Add(activeEffect);
            _handleToEffectMap[activeEffect.Handle.GetHashCode()] = activeEffect;
            if (spec.Period > 0.0f && spec.Definition.ExecutePeriodicEffect) {
                ExecutePeriodicEffect(activeEffect);
            }

            return activeEffect.Handle;
        }

        public bool RemoveActiveGameplayEffect(ActiveGameplayEffectHandle handle) {
            
        }

        private void ApplyModifiersToTarget(IActiveGameplayEffect effect, IGameplayEffectSpec spec) {
            
        }

        private void ApplyGrantedTags(IGameplayEffectSpec spec) {
            foreach (var tag in spec.Definition.GrantedTags.Tags) {
                _owner.TagContainer.AddTag(tag);
            }
        }

        private void GrantAbilities(IGameplayEffectSpec spec) {
            
        }

        private IActiveGameplayEffect FindStackableEffect(IGameplayEffectSpec spec) {
            return _activeEffects.FirstOrDefault(e => 
                e.Spec.Definition == spec.Definition &&
                e.Spec.Definition.StackingType != GameplayEffectStackingType.None
                );
        }
        
        private void ExecutePeriodicEffect(IActiveGameplayEffect effect) {
            
        }

        private void ExecuteInstantEffect(IGameplayEffectSpec spec) {
            spec.RecalculateModifiers(_owner);
            foreach (var mod in spec.EvaluatedModifiers) {
                
            }
        }

        private ActiveGameplayEffectHandle HandleStacking(IActiveGameplayEffect effect, IGameplayEffectSpec spec) {
            
        }

        public void Tick() {
            for (int i = _activeEffects.Count -1; i >= 0; i--) {
                var effect = _activeEffects[i];
                effect.AddPeriodElapsedTime(Time.deltaTime);
            }

            if (_expiredChecker is not null) {
                var expiredEffects = _expiredChecker.GetExpiredEffectHandles();
                if (expiredEffects.Count is not 0) {
                    foreach (var effect in expiredEffects) {
                        RemoveActiveGameplayEffect(effect);
                    }
                }
            }
            
            
        }
    }

    public class GameplayEffectExpiredCheckService {
        
        private readonly IActiveGameplayEffectContainer _container;
        
        private List<ActiveGameplayEffectHandle> _cachedResult = new List<ActiveGameplayEffectHandle>();
        
        public GameplayEffectExpiredCheckService(IActiveGameplayEffectContainer container) {
            _container = container;
        }

        public IReadOnlyList<ActiveGameplayEffectHandle> GetExpiredEffectHandles() {
            _cachedResult.Clear();
            
            if (_container is null) return _cachedResult;
            
            if (_container.ActiveEffects.Count is 0) {
                return _cachedResult;
            }
            
            foreach (var effect in _container.ActiveEffects) {
                if (effect is null) {
                    continue;
                }
                if (effect.IsExpired(Time.deltaTime)) {
                    _cachedResult.Add(effect.Handle);
                }
            }
            
            return _cachedResult;
        }
    }
}