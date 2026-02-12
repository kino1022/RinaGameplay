using System;

namespace RinaGameplay.Ability {

    public interface IGameplayAbility {
        
        IGameplayAbilityDefinition Definition { get; }
        
        IGameplayAbilitySpec CurrentSpec { get; }
        
        GameplayAbilitySystem CurrentAbilitySystem { get; }

        bool CanActivateAbility(IGameplayAbilitySpec spec, GameplayAbilitySystem asc);
        
        bool CheckCost(IGameplayAbilitySpec spec, GameplayAbilitySystem asc);
        
        bool CheckCoolDown(IGameplayAbilitySpec spec, GameplayAbilitySystem asc);
        
        void CommitCost (IGameplayAbilitySpec spec, GameplayAbilitySystem asc);
        
        void CommitCoolDown (IGameplayAbilitySpec spec, GameplayAbilitySystem asc);
        
        bool CommitAbility (IGameplayAbilitySpec spec, GameplayAbilitySystem asc);
        
        void ActivateAbility (IGameplayAbilitySpec spec, GameplayAbilitySystem asc);
        
        void EndAbility (bool wasCancelled);

    }
    
    public class GameplayAbility : IGameplayAbility {
        
        private IGameplayAbilityDefinition _definition;

        private IGameplayAbilitySpec _currentSpec;
        
        private GameplayAbilitySystem _currentAbilitySystem;
        
        public IGameplayAbilityDefinition Definition => _definition;
        
        public IGameplayAbilitySpec CurrentSpec => _currentSpec;
        
        public GameplayAbilitySystem CurrentAbilitySystem => _currentAbilitySystem;

        public GameplayAbility(IGameplayAbilityDefinition definition) {
            _definition = definition ?? throw new ArgumentNullException();
        }
        
        public bool CanActivateAbility (IGameplayAbilitySpec spec, GameplayAbilitySystem asc) {
            if (!Definition.ActivationRequirements.RequirementsMet(asc.TagContainer)) {
                return false;
            }
            if (!Definition.ActivationBlockers.RequirementsMet(asc.TagContainer)) {
                return false;
            }

            if (Definition.ActivateConditions.Count is not 0) {
                foreach (var condition in Definition.ActivateConditions) {
                    if (condition is null) {
                        continue;
                    }
                    if (!condition.ActivateCondition(spec, asc)) {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CheckCost(IGameplayAbilitySpec spec, GameplayAbilitySystem asc) {
            if (Definition.CostDefinitions.Count is 0) {
                return true;
            }
            foreach (var cost in Definition.CostDefinitions) {
                if (cost is null) {
                    continue;
                }
                if (!cost.CheckCost(spec, asc)) {
                    return false;
                }
            }
            return true;
        }

        public bool CheckCoolDown(IGameplayAbilitySpec spec, GameplayAbilitySystem asc) {
            if (Definition.CoolDownDefinitions.Count is 0) {
                return true;
            }

            foreach (var cooldown in Definition.CoolDownDefinitions) {
                if (cooldown is null) {
                    continue;
                }
                if (!cooldown.CheckCoolDown(spec, asc)) {
                    return false;
                }
            }
            return true;
        }

        public void CommitCost(IGameplayAbilitySpec spec, GameplayAbilitySystem asc) {
            if (Definition.CostDefinitions.Count is 0) {
                return;
            }
            foreach (var cost in Definition.CostDefinitions) {
                if (cost is null) {
                    continue;
                }
                cost.CommitCost(spec, asc);
            }
        }

        public void CommitCoolDown(IGameplayAbilitySpec spec, GameplayAbilitySystem asc) {
            if (Definition.CoolDownDefinitions.Count is 0) {
                return;
            }
            foreach (var cooldown in Definition.CoolDownDefinitions) {
                if (cooldown is null) {
                    continue;
                }
                cooldown.CommitCoolDown(spec, asc);
            }
        }

        public bool CommitAbility(IGameplayAbilitySpec spec, GameplayAbilitySystem asc) {
            if (!CheckCost(spec, asc) || !CheckCoolDown(spec, asc)) {
                return false;
            }
            CommitCost(spec, asc);
            CommitCoolDown(spec, asc);
            return true;
        }

        public void ActivateAbility(IGameplayAbilitySpec spec, GameplayAbilitySystem asc) {
            SetCurrentContext(spec, asc);
            spec.SetIsActive(true);
            foreach (var tag in Definition.ActivationOwnedTags.Tags) {
                asc.TagContainer.AddTag(tag);
            }
            Definition.ActivationAbilityInternal(spec, asc);
        }

        public void EndAbility(bool wasCancelled = false) {
            if (_currentSpec is null && _currentAbilitySystem is null) {
                return;
            }
            CurrentSpec.SetIsActive(false);
            foreach (var tag in Definition.ActivationOwnedTags.Tags) {
                _currentAbilitySystem.TagContainer.RemoveTag(tag);
            }
            Definition.EndAbilityInternal(CurrentSpec, CurrentAbilitySystem, wasCancelled);
            SetCurrentContext(null, null);
        }

        private void SetCurrentContext(IGameplayAbilitySpec spec, GameplayAbilitySystem asc) {
            SetCurrentSpec(spec);
            SetCurrentAbilitySystem(asc);
        }
        
        private void SetCurrentSpec(IGameplayAbilitySpec spec) {
            _currentSpec = spec;
        }
        
        private void SetCurrentAbilitySystem(GameplayAbilitySystem asc) {
            _currentAbilitySystem = asc;
        }
    }
}