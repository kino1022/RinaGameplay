using RinaGameplay.Attribute;
using RinaGameplay.Effect;
using RinaGameplay.Modifier.Definition;
using RinaGameplay.Tag.Container;
using Sirenix.Serialization;
using UnityEngine;

namespace RinaGameplay.Modifier {
    [System.Serializable]
    public class ModifierInfo {
        
        [OdinSerialize]
        public IGameplayAttribute Attribute;

        [SerializeField]
        public GameplayModifierOperator Operator = GameplayModifierOperator.Additive;

        [OdinSerialize]
        public IGameplayEffectMagnitudeCalculator Magnitude;
        
        [OdinSerialize]
        public IGameplayTagRequirements SourceRequiredTags = new GameplayTagRequirements();
        
        [OdinSerialize]
        public IGameplayTagRequirements TargetRequiredTags = new GameplayTagRequirements();
        
    }
}