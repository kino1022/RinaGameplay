using System.Collections.Generic;
using RinaGameplay.Tag.Container;
using Sirenix.OdinInspector;

namespace RinaGameplay.Ability {

    public interface IGameplayAbilityDefinition {
        
        IGameplayTagContainer AbilityTags { get; }
        
        IGameplayTagContainer ActivationTags { get; }
        
        IGameplayTagContainer BlockAbilitiesTag { get; }
        
        GameplayAbilityInstancePolicy InstancePolicy { get; }
        
        IReadOnlyList<IActivateAbilityConditionDefinition> ActivateConditions { get; }
        
        IReadOnlyList<IAbilityCostDefinition> CostDefinitions { get; }
        
        IReadOnlyList<IAbilityCoolDownDefinition> CoolDownDefinitions { get; }
        
        /// <summary>
        /// GameplayAbilityの発動中に所有するタグ
        /// </summary>
        IGameplayTagContainer ActivationOwnedTags { get; }

        /// <summary>
        /// GameplayAbilityの発動に必要なタグ要件
        /// </summary>
        IGameplayTagRequirements ActivationRequirements { get; }
        
        /// <summary>
        /// GameplayAbilityの発動を妨げるタグ要件
        /// </summary>
        IGameplayTagRequirements ActivationBlockers { get; }
        
        /// <summary>
        /// GameplayAbilityのインスタンスを生成する
        /// </summary>
        /// <returns></returns>
        IGameplayAbility CreateInstance();
        
        void ActivationAbilityInternal(IGameplayAbilitySpec spec, GameplayAbilitySystem asc);
        
        void EndAbilityInternal(IGameplayAbilitySpec spec, GameplayAbilitySystem asc, bool wasCancelled);
        
    }

    public interface IActivateAbilityConditionDefinition {
        bool ActivateCondition (IGameplayAbilitySpec spec, GameplayAbilitySystem asc);
    }
    
    public interface IAbilityCostDefinition {
        bool CheckCost (IGameplayAbilitySpec spec, GameplayAbilitySystem asc);
        
        void CommitCost (IGameplayAbilitySpec spec, GameplayAbilitySystem asc);
    }
    
    public interface IAbilityCoolDownDefinition {
        bool CheckCoolDown (IGameplayAbilitySpec spec, GameplayAbilitySystem asc);
        
        void CommitCoolDown (IGameplayAbilitySpec spec, GameplayAbilitySystem asc);
    }
    
    public abstract class GameplayAbilityDefinition : SerializedScriptableObject {
        
    }
    
}