using RinaGameplay.Ability.Container;
using RinaGameplay.Attribute;
using RinaGameplay.Effect.Container;
using RinaGameplay.Tag.Container;
using Sirenix.OdinInspector;

namespace RinaGameplay {
    public class AbilitySystemComponent : SerializedMonoBehaviour {

        protected IAttributeSet _attributeSet;

        protected IGameplayTagContainer _tags;
        
        protected IActiveGameplayEffectContainer _effects;
        
        protected IActiveAbilityContainer _abilities;

        public IGameplayTagContainer Tags => _tags;
        
        public IAttributeSet AttributeSet => _attributeSet; 
        
        public IActiveGameplayEffectContainer ActiveEffects => _effects;
        
        public IActiveAbilityContainer ActiveAbilities => _abilities;
        
        
    }
}