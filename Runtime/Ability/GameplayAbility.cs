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

        [OdinSerialize]
        [LabelText("アビリティキャンセルタグ")]
        [Tooltip("このタグが付与されたアビリティは、このアビリティによってキャンセルされる")]
        protected IGameplayTagContainer _cancelAbilityWithTag;

        [OdinSerialize]
        [LabelText("アビリティブロックタグ")]
        [Tooltip("このタグが付与されたアビリティは、このアビリティによってブロックされる")]
        protected IGameplayTagContainer _blockAbilityWithTag;

        [OdinSerialize]
        [LabelText("アクティブ時付与タグ")]
        [Tooltip("アビリティがアクティブな間に付与されるタグ")]
        protected IGameplayTagContainer _activationOwnedTags;

        [OdinSerialize]
        [LabelText("アクティベーション必要タグ")]
        [Tooltip("アビリティをアクティベートするために必要なタグ")]
        protected IGameplayTagRequirements _activationRequiredTags;

        [OdinSerialize]
        [LabelText("アクティベーションブロックタグ")]
        [Tooltip("このタグが付与されたアビリティは、このアビリティによってブロックされる")]
        protected IGameplayTagRequirements _activationBlockedTags;

        [OdinSerialize]
        [LabelText("コストの定義")]
        protected List<IAbilityCostDefinition> _costDefinitions = new List<IAbilityCostDefinition>();

        [OdinSerialize]
        [LabelText("クールダウンの定義")]
        protected List<IAbilityCoolDownDefinition> _coolDownDefinitions = new List<IAbilityCoolDownDefinition>();

        [OdinSerialize]
        [LabelText("タスク")]
        protected List<IAbilityTask> _tasks = new List<IAbilityTask>();

        [OdinSerialize]
        [LabelText("インスタンス化ポリシー")]
        protected AbilityInstancingPolicy _instancingPolicy = AbilityInstancingPolicy.NonInstance;

        public IGameplayTagContainer AbilityTags => _abilityTags;

        public IGameplayTagContainer CancelAbilityWithTag => _cancelAbilityWithTag;

        public IGameplayTagContainer BlockAbilityWithTag => _blockAbilityWithTag;

        public IGameplayTagContainer ActivationOwnedTags => _activationOwnedTags;

        public IGameplayTagRequirements ActivationRequiredTags => _activationRequiredTags;

        public IGameplayTagRequirements ActivationBlockedTags => _activationBlockedTags;

        public IReadOnlyList<IAbilityCostDefinition> CostDefinitions => _costDefinitions;

        public IReadOnlyList<IAbilityCoolDownDefinition> CoolDownDefinitions => _coolDownDefinitions;

        public IReadOnlyList<IAbilityTask> Tasks => _tasks;

        public AbilityInstancingPolicy InstancingPolicy => _instancingPolicy;

        public virtual void ActivateAbilityInternal(IGameplayAbilitySpec spec, AbilitySystemComponent asc) {
            // デフォルトの実装はなし。必要に応じてオーバーライドして使用。
        }
        
        public virtual void EndAbilityInternal(IGameplayAbilitySpec spec, AbilitySystemComponent asc, bool isCancel) {
            // デフォルトの実装はなし。必要に応じてオーバーライドして使用。
        }
        
        public abstract IActiveGameplayAbility CreateInstance();
    }
    
}