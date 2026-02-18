using RinaGameplay.Ability.Container;
using RinaGameplay.Attribute;
using RinaGameplay.Effect.Container;
using RinaGameplay.Tag.Container;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace RinaGameplay {
    public class AbilitySystemComponent : SerializedMonoBehaviour {

        [OdinSerialize]
        [ReadOnly]
        protected IAttributeSet _attributeSet;

        [OdinSerialize]
        protected IGameplayTagContainer _tags;

        [OdinSerialize]
        [ReadOnly]
        protected IActiveGameplayEffectContainer _effects;
        
        [OdinSerialize]
        [ReadOnly]
        protected IActiveAbilityContainer _abilities;

        public IGameplayTagContainer Tags => _tags;
        
        public IAttributeSet AttributeSet => _attributeSet; 
        
        public IActiveGameplayEffectContainer ActiveEffects => _effects;

        public IActiveAbilityContainer ActiveAbilities => _abilities;

        private void Awake() {
            _tags ??= new GameplayTagContainer();
            _effects = new ActiveGameplayEffectContainer(this);
            _abilities = new ActiveAbilityContainer(this);
            _attributeSet = new AttributeSet();
        }


    }
}