using System.Collections.Generic;
using RinaGameplay.Ability.Definition;
using RinaGameplay.Effect;
using RinaGameplay.Tag.Container;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace RinaGameplay.Ability {
    public interface IGameplayAbility {
        
        IGameplayTagContainer AbilityTags { get; }
        
        IGameplayTagContainer CancelAbilityWithTag { get; }
        
        IGameplayTagContainer BlockAbilityWithTag { get; }
        
        IGameplayTagContainer ActivationOwnedTags { get; }
        
        IGameplayTagRequirements ActivationRequiredTags { get; }
        
        IGameplayTagRequirements ActivationBlockedTags { get; }
        
        IReadOnlyList<IAbilityCostDefinition> CostDefinitions { get; }
        
        IReadOnlyList<IAbilityCoolDownDefinition> CoolDownDefinitions { get; }
        
        IReadOnlyList<IAbilityTask> Tasks { get; }
        
        AbilityInstancingPolicy InstancingPolicy { get; }
        
        void ActivateAbilityInternal(IGameplayAbilitySpec spec, AbilitySystemComponent asc);
        
        void EndAbilityInternal(IGameplayAbilitySpec spec, AbilitySystemComponent asc, bool isCancel);

        bool CheckCost(IGameplayAbilitySpec spec, AbilitySystemComponent asc) {
            if (CostDefinitions.Count == 0) return true;
            foreach (var def in CostDefinitions) {
                if (def is null) continue;
                if (!def.CheckCost(spec, asc)) {
                    return false;
                }
            }
            return true;
        }
        
        bool CheckCoolDown(IGameplayAbilitySpec spec, AbilitySystemComponent asc) {
            if (CoolDownDefinitions.Count == 0) return true;
            foreach (var def in CoolDownDefinitions) {
                if (def is null) continue;
                if (!def.CheckCoolDown(spec, asc)) {
                    return false;
                }
            }
            return true;
        }
        
        void CommitCost(IGameplayAbilitySpec spec, AbilitySystemComponent asc) {
            foreach (var def in CostDefinitions) {
                if (def is null) continue;
                def.CommitCost(spec, asc);
            }
        }

        void CommitCoolDown(IGameplayAbilitySpec spec, AbilitySystemComponent asc) {
             foreach (var def in CoolDownDefinitions) {
                 if (def is null) continue;
                    def.CommitCoolDown(spec, asc);
             }
        }

        IActiveGameplayAbility CreateInstance();

    }

    public interface IAbilityCostDefinition {
        
        bool CheckCost (IGameplayAbilitySpec spec, AbilitySystemComponent asc);
        
        void CommitCost (IGameplayAbilitySpec spec, AbilitySystemComponent asc);
        
    }
    
    public interface IAbilityCoolDownDefinition {
        
        bool CheckCoolDown (IGameplayAbilitySpec spec, AbilitySystemComponent asc);
        
        void CommitCoolDown (IGameplayAbilitySpec spec, AbilitySystemComponent asc);
        
    }

    public abstract class GameplayAbility : SerializedScriptableObject {

        [OdinSerialize]
        [LabelText("アビリティタグ")]
        [Tooltip("アビリティ自体に付与されるタグ")]
        protected IGameplayTagContainer _abilityTags;
        
        
        protected IGameplayTagContainer _cancelAbilityWithTag;
        
        
        public abstract IActiveGameplayAbility CreateInstance();
    }
    
}