using System;
using System.Collections.Generic;
using ObservableCollections;
using R3;
using RinaGameplay.Ability;
using RinaGameplay.Effect;
using RinaGameplay.Tag;
using RinaGameplay.Tag.Container;
using Sirenix.OdinInspector;

namespace RinaGameplay {
    
    public interface IGameplayAbilitySystem {
        
        IGameplayTagContainer TagContainer { get; }
        
        IActiveGameplayEffectContainer ActiveEffectContainer { get; }
        
        event Action<GameplayTag> OnTagAdded;
        
        event Action<GameplayTag> OnTagRemoved;

        GameplayAbilitySpecHandle GiveAbility(IGameplayAbilitySpec spec);
        
        GameplayAbilitySpecHandle GiveAbilityActivateOnce (IGameplayAbilityDefinition def, int level = 1);

        bool TryActivateAbility(GameplayAbilitySpecHandle handle);

        IGameplayAbility GetOrCreateAbilityInstance(IGameplayAbilitySpec spec);
        
        void CancelAbility (GameplayAbilitySpecHandle handle);

        ActiveGameplayEffectHandle ApplyGameplayEffectSpecToSelf(IGameplayEffectSpec spec);
        
        ActiveGameplayEffectHandle ApplyGameplayEffectSpecToTarget(IGameplayEffectSpec spec, IGameplayAbilitySystem targetAsc);
        
        ActiveGameplayEffectHandle ApplyGameplayEffectToSelf(IGameplayEffect def, float level = 1.0f);
        
        ActiveGameplayEffectHandle ApplyGameplayEffectToTarget(IGameplayEffect def, IGameplayAbilitySystem targetAsc, float level = 1.0f);
        
        bool RemoveActiveGameplayEffect (ActiveGameplayEffectHandle handle);

    }
    
    public class GameplayAbilitySystem : SerializedMonoBehaviour, IGameplayAbilitySystem {
        
        private IObservableGameplayTagContainer _tagContainer;
        
        private IActiveGameplayEffectContainer _activeEffectContainer;
        
        private List<IGameplayAbilitySpec> _grantedAbilities = new List<IGameplayAbilitySpec>();
        private Dictionary<int, IGameplayAbilitySpec> _handleToAbilitySpecMap = new Dictionary<int, IGameplayAbilitySpec>();
        
        private bool _isRegisteredOnTagChanged = false;
        
        public IGameplayTagContainer TagContainer => _tagContainer;
        
        public IActiveGameplayEffectContainer ActiveEffectContainer => _activeEffectContainer;
        
        public event Action<GameplayTag> OnTagAdded;
        public event Action<GameplayTag> OnTagRemoved;

        private void Awake() {
            //grant処理とかはここでやるべき
            RegisterOnTagChanged();
        }

        private void Start() {
            
        }

        private void Update() {
            if (!_isRegisteredOnTagChanged) {
                RegisterOnTagChanged();
            }
        }

        public GameplayAbilitySpecHandle GiveAbility(IGameplayAbilitySpec spec) {
            _grantedAbilities.Add(spec);
            _handleToAbilitySpecMap[spec.Handle.GetHashCode()] = spec;
            if (spec.Definition.InstancePolicy == GameplayAbilityInstancePolicy.InstancedPerActor) {
                spec.SetAbilityInstance(spec.Definition.CreateInstance());
            }
            return spec.Handle;
        }

        public GameplayAbilitySpecHandle GiveAbilityActivateOnce(IGameplayAbilityDefinition def, int level = 1) {
            var spec = new GameplayAbilitySpec(def, level);
            var handle = GiveAbility(spec);
            TryActivateAbility(handle);
            return handle;
        }

        public bool TryActivateAbility(GameplayAbilitySpecHandle handle) {
            if (!_handleToAbilitySpecMap.TryGetValue(handle.GetHashCode(), out var spec)) {
                return false;
            }
            IGameplayAbility instance = GetOrCreateAbilityInstance(spec);
            if (!instance.CanActivateAbility(spec, this)) {
                return false;
            }
            if (!instance.CheckCost(spec, this)) {
                return false;
            }
            if (!instance.CheckCoolDown(spec, this)) {
                return false;
            }
            instance.ActivateAbility(spec, this);
            return true;
        }

        public IGameplayAbility GetOrCreateAbilityInstance(IGameplayAbilitySpec spec) {
            switch (spec.Definition.InstancePolicy) {
                case GameplayAbilityInstancePolicy.NonInstanced:
                    if (spec.Instance is null) {
                        spec.SetAbilityInstance(spec.Definition.CreateInstance());
                    }
                    return spec.Instance;
                case GameplayAbilityInstancePolicy.InstancedPerActor:
                    return spec.Instance;
                case GameplayAbilityInstancePolicy.InstancedPerExecution:
                    return spec.Definition.CreateInstance();
                default:
                    return spec.Instance;
            }
        }

        public void CancelAbility(GameplayAbilitySpecHandle handle) {
            if (_handleToAbilitySpecMap.TryGetValue(handle.GetHashCode(), out var spec)) {
                spec.Instance?.EndAbility(true);
            }
        }

        private void RegisterOnTagChanged() {
            if (_tagContainer is null) {
                return;
            }

            _tagContainer
                .ObservableTags
                .ObserveAdd()
                .Subscribe(x => OnTagAdded?.Invoke(x.Value));
            
            _tagContainer
                .ObservableTags
                .ObserveRemove()
                .Subscribe(x => OnTagRemoved?.Invoke(x.Value));
            
            _isRegisteredOnTagChanged = true;
        }
    }
}