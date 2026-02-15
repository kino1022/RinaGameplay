using System.Collections.Generic;
using RinaGameplay.Effect.Definition;
using RinaGameplay.Modifier;
using RinaGameplay.Tag;
using UnityEngine;

namespace RinaGameplay.Effect {
    public interface IGameplayEffectSpec {
        
        /// <summary>
        /// 発動するGameplayEffectの定義
        /// </summary>
        IGameplayEffect Definition { get; }
        
        /// <summary>
        /// 発動主と発動対象などの情報を管理する構造体
        /// </summary>
        GameplayEffectContextHandle Handle { get; }
        
        /// <summary>
        /// GameplayEffectの効果レベル
        /// </summary>
        float Level { get; }
        
        /// <summary>
        /// 効果の持続時間
        /// </summary>
        float Duration { get; }
        
        /// <summary>
        /// 効果の発動周期
        /// </summary>
        float Period { get; }
        
        /// <summary>
        /// 現在のスタック数
        /// </summary>
        int StackCount { get; }
        
        IReadOnlyList<EvaluatedModifier> EvaluatedModifiers { get; }
        
        /// <summary>
        /// タグごとの値をセットする
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="magnitude"></param>
        void SetSetByCallerMagnitude (GameplayTag tag, float magnitude);
        
        /// <summary>
        /// タグに対応した値を取得する
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        float GetSetByCallerMagnitude (GameplayTag tag);

        /// <summary>
        /// GameplayEffectのModifierの効果料を再適用する
        /// </summary>
        /// <param name="target"></param>
        void RecalculateModifiers(AbilitySystemComponent target);

    }

    /// <summary>
    /// GameplayEffectの発動対象や発動元などに関する情報を保持する構造体
    /// </summary>
    public struct GameplayEffectContextHandle {
        
        public GameObject SourceObject;
        
        public AbilitySystemComponent SourceAbilitySystem;
        
        public GameObject TargetObject;
        
        public AbilitySystemComponent TargetAbilitySystem;

    }

    public class GameplayEffectSpec : IGameplayEffectSpec {
        
        protected GameplayEffectContextHandle _handle;
        
        protected List<EvaluatedModifier> _evaluatedModifiers;
        
        public IGameplayEffect Definition { get; private set; }
        
        public GameplayEffectContextHandle Handle => _handle;
        
        public float Level { get; protected set; }
        
        public float Duration { get; protected set; }
        
        public float Period { get; protected set; }
        
        public int StackCount { get; protected set; }
        
        public IReadOnlyList<EvaluatedModifier> EvaluatedModifiers => _evaluatedModifiers;

        private Dictionary<GameplayTag, float> _setByCaller;
        
        public GameplayEffectSpec(IGameplayEffect definition, GameObject source, float level = 1) {
            Definition = definition;
            if (source.TryGetComponent(out AbilitySystemComponent asc)) {
                _handle.SourceAbilitySystem = asc;
                _handle.SourceObject = source;
            }
            else {
                _handle.SourceObject = source;
            }
            Level = level;
            CalculateDurationAndPeriod();
        }
        
        public void SetSetByCallerMagnitude(GameplayTag tag, float magnitude) {
            _setByCaller[tag] = magnitude;
        }

        public float GetSetByCallerMagnitude(GameplayTag tag) {
            if (_setByCaller.TryGetValue(tag, out var magnitude)) {
                return magnitude;
            }
            else {
                Debug.LogWarning($"SetByCallerMagnitude for tag {tag} not found.");
                return 0.0f;
            }
        }

        public void RecalculateModifiers(AbilitySystemComponent target) {
            _handle.TargetAbilitySystem = target;
            _handle.TargetObject = target.gameObject;
            _evaluatedModifiers.Clear();
            foreach (var mod in Definition.Modifiers) {
                if (mod is null) {
                    continue;
                }

                //　発動元がAbilitySystemを持っていない場合は環境からの効果発動と見なしてタグの判定をスキップする
                if (_handle.SourceAbilitySystem is not null) {
                    if (!mod.SourceRequiredTags.RequirementMet(_handle.SourceAbilitySystem.Tags)) {
                        continue;
                    }
                }

                if (!mod.TargetRequiredTags.RequirementMet(_handle.TargetAbilitySystem.Tags)) {
                    continue;
                }
                
                float magnitude = mod.Magnitude.CalculateMagnitude(this, _handle.SourceAbilitySystem, _handle.TargetAbilitySystem);
                
                _evaluatedModifiers.Add(new EvaluatedModifier {
                    Attribute = mod.Attribute,
                    Operator = mod.Operator,
                    Magnitude = magnitude
                });
            }
        }

        private void CalculateDurationAndPeriod() {
            InitDuration();
            InitPeriod();
        }

        private void InitDuration() {

            if (Definition.DurationDef != null && Definition.DurationDef.DurationType == EffectDurationType.Duration) {
                Duration = Definition.DurationDef.GetDuration(this);
            }

            Duration = 0.0f;
        }
        
        private void InitPeriod() {

            if (Definition.PeriodicDef != null && Definition.PeriodicDef.ExecutePeriodic) {
                Period = Definition.PeriodicDef.GetPeriod(this);
            }

            Period = 0.0f;
        }
        
    }
}