using System.Collections.Generic;
using RinaGameplay.Effect.Definition;
using RinaGameplay.Effect.Exception;
using RinaGameplay.Modifier;
using RinaGameplay.Modifier.Definition;
using UnityEngine;

namespace RinaGameplay.Effect.Container {

    public interface IActiveGameplayEffectContainer {
        
        /// <summary>
        /// このコンテナに格納されているアクティブなGameplayEffectのリスト
        /// </summary>
        IReadOnlyList<IActiveGameplayEffect> Effects { get; }

        /// <summary>
        /// GameplayEffectSpecをもとにして、GameplayEffectをこのコンテナに対して適応する
        /// </summary>
        /// <param name="spec"></param>
        /// <returns>適用したGameplayEffectのハンドル,適用に失敗した場合はInValidなハンドルを返す</returns>
        ActiveGameplayEffectHandle ApplyGameplayEffectSpec(IGameplayEffectSpec spec);
        
        /// <summary>
        /// ハンドルをもとにして対応するActiveGameplayEffectを解除してこのコンテナから削除する
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        bool RemoveActiveGameplayEffect(ActiveGameplayEffectHandle handle);

    }

    public class ActiveGameplayEffectContainer : IActiveGameplayEffectContainer {

        private readonly AbilitySystemComponent _owner;

        private List<IActiveGameplayEffect> _effects;

        private Dictionary<int, IActiveGameplayEffect> _handleToEffectMap;

        public IReadOnlyList<IActiveGameplayEffect> Effects => _effects;

        public ActiveGameplayEffectContainer(AbilitySystemComponent owner) {
            _owner = owner;
            _effects = new List<IActiveGameplayEffect>();
            _handleToEffectMap = new Dictionary<int, IActiveGameplayEffect>();
        }

        public ActiveGameplayEffectHandle ApplyGameplayEffectSpec(IGameplayEffectSpec spec) {
            if (!spec.Definition.CanApply(_owner.Tags)) {
                return ActiveGameplayEffectHandle.InValid;
            }

            if (spec.Definition.DurationDef is not null) {
                if (spec.Definition.DurationDef.DurationType == EffectDurationType.Instant) {
                    ExecuteInstantEffect(spec);
                    return ActiveGameplayEffectHandle.InValid;
                }
            }
            else {
                return ActiveGameplayEffectHandle.InValid;
            }

            if (spec.Definition.StackingDef is not null) {
                if (spec.Definition.StackingDef.StackingPolicy == EffectStackingPolicy.None) {
                    var existingEffect = FindStackableEffect(spec);
                    if (existingEffect is not null) {
                        return HandleStacking(existingEffect, spec);
                    }
                }
            }

            var activeEffect = new ActiveGameplayEffect(spec);
            activeEffect.SetStartTime(Time.time);
            spec.RecalculateModifiers(_owner);
            ApplyModifierToTarget(activeEffect, spec);
            ApplyGrantedTags(spec);
            ApplyGrantedAbilities(spec);
            _effects.Add(activeEffect);
            _handleToEffectMap[activeEffect.Handle.GetHashCode()] = activeEffect;
            if (activeEffect.ShouldExecutePeriodicEffect(Time.time)) {
                ExecutePeriodicEffect(activeEffect);
            }

            return activeEffect.Handle;
        }

        public bool RemoveActiveGameplayEffect(ActiveGameplayEffectHandle handle) {
            if (!_handleToEffectMap.TryGetValue(handle.GetHashCode(), out var effect)) {
                return false;
            }

            RemoveModifierFromTarget(effect);
            RemoveGrantedTags(effect.Spec);
            _effects.Remove(effect);
            _handleToEffectMap.Remove(handle.GetHashCode());
            return true;
        }

        /// <summary>
        /// 即時発動する効果を実行する
        /// </summary>
        /// <param name="spec"></param>
        private void ExecuteInstantEffect(IGameplayEffectSpec spec) {
            spec.RecalculateModifiers(_owner);
            foreach (var eval in spec.EvaluatedModifiers) {
                var attribute = _owner.AttributeSet.GetAttributeValue(eval.Attribute);
                if (attribute is null) {
                    continue;
                }

                float current = attribute.CurrentValue.CurrentValue;
                float next = ApplyModifierOperationToTarget(current, eval.Magnitude, eval.Operator);
                attribute.SetBaseValue(next);
            }
        }

        /// <summary>
        /// 周期的な効果を実行する
        /// </summary>
        /// <param name="effect"></param>
        private void ExecutePeriodicEffect(IActiveGameplayEffect effect) {
            ExecuteInstantEffect(effect.Spec);
        }

        /// <summary>
        /// 渡されたSpecに対応するGameplayEffectを取得する
        /// </summary>
        /// <param name="spec"></param>
        /// <returns>対応するActiveGameplayEffect、存在しない場合はnullが返ることに留意</returns>
        private IActiveGameplayEffect FindStackableEffect(IGameplayEffectSpec spec) {

            if (spec.Definition.StackingDef is null) {
                return null;
            }
            
            return _effects.Find(x => x.Spec.Definition == spec.Definition &&
                                      x.Spec.Definition.StackingDef.StackingPolicy != EffectStackingPolicy.None);
        }

        private ActiveGameplayEffectHandle HandleStacking(IActiveGameplayEffect effect, IGameplayEffectSpec spec) {
            if (effect.Spec.Definition.StackingDef.MaxStack > 0 &&
                effect.Spec.StackCount >= effect.Spec.Definition.StackingDef.MaxStack) {
                return effect.Handle;
            }

            effect.Spec.StackCount++;
            if (effect.Spec.Definition.StackingDef.OnStackDuration == EffectStackingDurationPolicy.RefreshOnStack) {
                effect.SetStartTime(Time.time);
            }

            if (effect.Spec.Definition.StackingDef.OnStackPeriod == EffectStackingPeriodPolicy.RefreshOnStack) {
                effect.SetPeriodElapsed(0.0f);
            }
            return effect.Handle;
        }

        private void ApplyModifierToTarget(IActiveGameplayEffect effect, IGameplayEffectSpec spec) {
            foreach (var eval in spec.EvaluatedModifiers) {
                var attribute = _owner.AttributeSet.GetAttributeValue(eval.Attribute);
                if (attribute is null) {
                    continue;
                }

                var activeModifier = new ActiveModifier(eval.Attribute, eval.Operator, eval.Magnitude, effect.Handle);
                attribute.AddModifier(ref activeModifier);
                effect.AppliedModifiers.Add(activeModifier);
            }
        }

        private void ApplyGrantedTags(IGameplayEffectSpec spec) {
            foreach (var tag in spec.Definition.GrantTags.Tags) {
                _owner.Tags.AddTag(tag);
            }
        }

        private void ApplyGrantedAbilities(IGameplayEffectSpec spec) {
            foreach (var ability in spec.Definition.GrantedAbilities) {
                
            }
        }

        private float ApplyModifierOperationToTarget(float baseValue, float magnitude, GameplayModifierOperator op) {
            return op.ApplyOperator(baseValue, magnitude);
        }

        private void RemoveModifierFromTarget(IActiveGameplayEffect effect) {
            for (int i = 0; i < effect.AppliedModifiers.Count; i++) {
                var mod = effect.AppliedModifiers[i];
                var attribute = _owner.AttributeSet.GetAttributeValue(mod.Target);
                if (attribute is null) {
                    continue;
                }
                attribute.RemoveModifier(ref mod);
            }
        }
        
        private void RemoveGrantedTags(IGameplayEffectSpec spec) {
            foreach (var tag in spec.Definition.GrantTags.Tags) {
                _owner.Tags.RemoveTag(tag);
            }
        }
        
    }
}