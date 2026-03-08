using RinaGameplay.Attribute;
using RinaGameplay.Effect;
using RinaGameplay.Modifier.Definition;
using RinaGameplay.Tag.Container;
using Sirenix.Serialization;
using UnityEngine;

namespace RinaGameplay.Modifier {
    /// <summary>
    /// GameplayEffectでModifierを定義するためのクラス
    /// </summary>
    [System.Serializable]
    public class ModifierInfo {
        
        [OdinSerialize]
        [Tooltip("Modifierの対象になるGameplayAttribute")]
        public IGameplayAttribute Attribute;

        [SerializeField]
        [Tooltip("Modifierの演算方法")]
        public GameplayModifierOperator Operator = GameplayModifierOperator.Additive;

        [OdinSerialize]
        [Tooltip("Modifierの効果量を計算するためのクラス")]
        public IGameplayEffectMagnitudeCalculator Magnitude;
        
        [OdinSerialize]
        [Tooltip("Modifierの発生源が満たすべきタグ条件")]
        public IGameplayTagRequirements SourceRequiredTags = new GameplayTagRequirements();
        
        [OdinSerialize]
        [Tooltip("Modifierの対象が満たすべきタグ条件")]
        public IGameplayTagRequirements TargetRequiredTags = new GameplayTagRequirements();
        
    }
}