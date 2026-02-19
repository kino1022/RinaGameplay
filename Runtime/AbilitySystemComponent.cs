using System;
using System.Collections.Generic;
using RinaGameplay.Ability;
using RinaGameplay.Ability.Container;
using RinaGameplay.Attribute;
using RinaGameplay.Effect.Container;
using RinaGameplay.Tag.Container;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

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
        
        [Title("初期付与")]
        
        [OdinSerialize]
        [LabelText("初期アビリティ")]
        private List<IGameplayAbilitySpec> _initAbilities = new();
        
        [OdinSerialize]
        [LabelText("初期アトリビュート")]
        private Dictionary<IGameplayAttribute, float> _initAttributes = new();

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

        private void Start() {
            AbilitiesInitialize();
            AttributesInitialize();
        }

        private void Update() {
            _effects.Tick(Time.deltaTime);
        }

        private void AbilitiesInitialize() {
            _abilities ??= new ActiveAbilityContainer(this);
            if (_initAbilities.Count is 0) {
                return;
            }
            foreach (var ability in _initAbilities) {
                if (ability is null) continue;
                _abilities.GiveAbility(ability);
            }
        }

        private void AttributesInitialize() {
            _attributeSet ??= new AttributeSet();
            if (_initAttributes.Count is 0) {
                return;
            }

            foreach (var attribute in _initAttributes) {
                if (attribute.Key is null) continue;
                _attributeSet.GiveAttributeValue(attribute.Key, attribute.Value);
            }
        }
    }
}