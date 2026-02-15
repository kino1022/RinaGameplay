using RinaGameplay.Attribute;
using RinaGameplay.Effect;
using RinaGameplay.Modifier.Definition;
using RinaGameplay.Tag.Container;

namespace RinaGameplay.Modifier {
    public class ModifierInfo {
        public IGameplayAttribute Attribute;
        
        public GameplayModifierOperator Operator;

        public IGameplayEffectMagnitudeCalculator Magnitude;
        
        public IGameplayTagRequirements SourceRequiredTags;
        
        public IGameplayTagRequirements TargetRequiredTags;
        
    }
}