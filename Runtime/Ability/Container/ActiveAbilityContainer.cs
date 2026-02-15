using System.Collections.Generic;
using RinaGameplay.Ability.Definition;

namespace RinaGameplay.Ability.Container {
    public interface IActiveAbilityContainer {
        
        IReadOnlyList<IGameplayAbilitySpec> ActiveAbilities { get; }

        bool TryActivateAbility(GameplayAbilitySpecHandle handle);
        
        GameplayAbilitySpecHandle GiveAbility(IGameplayAbilitySpec spec);

        GameplayAbilitySpecHandle GiveAbilityAndActivateOnce(IGameplayAbility ability, int level = 1);
        
        void CancelAbility(GameplayAbilitySpecHandle handle);
        
    } 
    
    public class ActiveAbilityContainer : IActiveAbilityContainer {

        private readonly AbilitySystemComponent _owner;

        private Dictionary<int, IGameplayAbilitySpec> _handleToSpecMap = new();

        private List<IGameplayAbilitySpec> _abilities = new();
        
        public IReadOnlyList<IGameplayAbilitySpec> ActiveAbilities => _abilities;

        public ActiveAbilityContainer(AbilitySystemComponent owner) {
            _owner = owner;
        }

        public GameplayAbilitySpecHandle GiveAbility(IGameplayAbilitySpec spec) {
            _abilities.Add(spec);
            _handleToSpecMap[spec.Handle.GetHashCode()] = spec;
            if (spec.Definition.InstancingPolicy == AbilityInstancingPolicy.InstancePerActor) {
                spec.AbilityInstance = spec.Definition.CreateInstance();
            }

            return spec.Handle;
        }

        public GameplayAbilitySpecHandle GiveAbilityAndActivateOnce(IGameplayAbility ability, int level = 1) {
            var spec = new GameplayAbilitySpec(ability, level);
            var handle = GiveAbility(spec);
            TryActivateAbility(handle);
            return handle;
        }

        public bool TryActivateAbility(GameplayAbilitySpecHandle handle) {
            if (!_handleToSpecMap.TryGetValue(handle.GetHashCode(), out var spec)) {
                return false;
            }

            IActiveGameplayAbility instance = GetOrCreateAbilityInstance(spec);

            if (!instance.CanActivateAbility(spec, _owner)) {
                return false;
            }

            if (!instance.CheckCost(spec, _owner)) {
                return false;
            }

            if (!instance.CheckCoolDown(spec, _owner)) {
                return false;
            }
            
            instance.ActivateAbility(spec, _owner);
            return true;
        }

        public void CancelAbility(GameplayAbilitySpecHandle handle) {
            if (_handleToSpecMap.TryGetValue(handle.GetHashCode(), out var spec)) {
                spec.AbilityInstance.EndAbility(spec, _owner, true);
            }
        }

        private IActiveGameplayAbility GetOrCreateAbilityInstance(IGameplayAbilitySpec spec) {
            switch (spec.Definition.InstancingPolicy) {
                case AbilityInstancingPolicy.NonInstance:
                    if (spec.AbilityInstance is null) {
                        spec.AbilityInstance = spec.Definition.CreateInstance();
                    }
                    return spec.AbilityInstance;
                case AbilityInstancingPolicy.InstancePerActor:
                    return spec.AbilityInstance;
                case AbilityInstancingPolicy.InstancePerExecution:
                    return spec.Definition.CreateInstance();
                default:
                    return spec.AbilityInstance;
            }
        }
    }
}