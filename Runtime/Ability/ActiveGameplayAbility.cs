using System.Threading;

namespace RinaGameplay.Ability {

    public interface IActiveGameplayAbility {
        
        IGameplayAbility Definition { get; }
        
        IGameplayAbilitySpec CurrentSpec { get; }
        
        AbilitySystemComponent CurrentActor { get; }
        
        bool IsActive => CurrentSpec?.IsActive ?? false;

        bool CanActivateAbility(IGameplayAbilitySpec spec, AbilitySystemComponent asc);
        
        bool CheckCost (IGameplayAbilitySpec spec, AbilitySystemComponent asc);
        
        bool CheckCoolDown (IGameplayAbilitySpec spec, AbilitySystemComponent asc);
        
        void CommitCost (IGameplayAbilitySpec spec, AbilitySystemComponent asc);
        
        void CommitCoolDown (IGameplayAbilitySpec spec, AbilitySystemComponent asc);
        
        bool CommitAbility (IGameplayAbilitySpec spec, AbilitySystemComponent asc);
        
        void ActivateAbility (IGameplayAbilitySpec spec, AbilitySystemComponent asc);
        
        void EndAbility (IGameplayAbilitySpec spec, AbilitySystemComponent asc, bool isCancel);
        
    }
    
    public class ActiveGameplayAbility : IActiveGameplayAbility {
        
        public IGameplayAbility Definition { get; protected set; }
        
        public IGameplayAbilitySpec CurrentSpec { get; protected set; }
        
        public AbilitySystemComponent CurrentActor { get; protected set; }

        private IActiveAbilityTaskContainer _taskContainer;

        public ActiveGameplayAbility(IGameplayAbility def) {
            Definition = def;
        }
        
        public void ActivateAbility (IGameplayAbilitySpec spec, AbilitySystemComponent asc) {
            SetCurrentState(spec, asc);
            CurrentSpec.IsActive = true;
            if (Definition.Tasks.Count > 0) {
                foreach (var task in Definition.Tasks) {
                    if (task is null) continue;
                    _taskContainer.AddTask(task);
                }
                _taskContainer.ActivateAllTask(this);
            }
            ApplyGrantedTags(asc);
            Definition.ActivateAbilityInternal(CurrentSpec, CurrentActor);
        }

        public void EndAbility(IGameplayAbilitySpec spec, AbilitySystemComponent asc, bool isCancel) {
            if (CurrentSpec is null || CurrentActor is null) {
                return;
            }

            CurrentSpec.IsActive = false;
            
            _taskContainer.CancelAllTasks();
            _taskContainer.RemoveAllTasks();
            
            RemoveGrantedTags(asc);
            
            Definition.EndAbilityInternal(spec, CurrentActor, isCancel);

            SetCurrentState(null, null);
        }

        public bool CheckCost(IGameplayAbilitySpec spec, AbilitySystemComponent asc) {
            if (Definition is null) return false;
            if (Definition.CostDefinitions.Count == 0) return true;
            return Definition.CheckCost(spec, asc);
        }
        
        public bool CheckCoolDown(IGameplayAbilitySpec spec, AbilitySystemComponent asc) {
            if (Definition is null) return false;
            if (Definition.CoolDownDefinitions.Count == 0) return true;
            return Definition.CheckCoolDown(spec, asc);
        }

        public bool CanActivateAbility(IGameplayAbilitySpec spec, AbilitySystemComponent asc) {
            if (!Definition.ActivationRequiredTags.RequirementMet(asc.Tags)) {
                return false;
            }

            if (!Definition.ActivationBlockedTags.RequirementMet(asc.Tags)) {
                return false;
            }

            return true;
        }

        public void CommitCoolDown(IGameplayAbilitySpec spec, AbilitySystemComponent asc) {
            if (Definition is null) return;
            Definition.CommitCoolDown(spec, asc);
        }
        
        public void CommitCost(IGameplayAbilitySpec spec, AbilitySystemComponent asc) {
            if (Definition is null) return;
            Definition.CommitCost(spec, asc);
        }

        public bool CommitAbility(IGameplayAbilitySpec spec, AbilitySystemComponent asc) {
            if (!CheckCost(spec, asc) || !CheckCoolDown(spec, asc)) return false; 
            CommitCost(spec, asc);
            CommitCoolDown(spec, asc);
            return true;
        }

        private void SetCurrentState(IGameplayAbilitySpec spec, AbilitySystemComponent asc) {
            SetCurrentSpec(spec);
            SetCurrentActor(asc);
        }
        
        private void SetCurrentSpec (IGameplayAbilitySpec spec) {
            CurrentSpec = spec;
        }
        
        private void SetCurrentActor (AbilitySystemComponent asc) {
            CurrentActor = asc;
        }

        private void ApplyGrantedTags(AbilitySystemComponent asc) {
            foreach (var tag in Definition.ActivationOwnedTags.Tags) {
                asc.Tags.AddTag(tag);
            }
        }
        
        private void RemoveGrantedTags(AbilitySystemComponent asc) {
            foreach (var tag in Definition.ActivationOwnedTags.Tags) {
                asc.Tags.RemoveTag(tag);
            }
        }
    }
}