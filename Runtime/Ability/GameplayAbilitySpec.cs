using System.Threading;

namespace RinaGameplay.Ability {

    public interface IGameplayAbilitySpec {
        
        IGameplayAbilityDefinition Definition { get; }
        
        GameplayAbilitySpecHandle Handle { get; }
        
        float Level { get; }
        
        int InputID { get; }
        
        bool IsActive { get; }

        IGameplayAbility Instance { get; }
        
        CancellationTokenSource Cts { get; }
        
        bool SetAbilityInstance (IGameplayAbility instance);

        void SetIsActive(bool isActive);

    }
    
    public class GameplayAbilitySpec : IGameplayAbilitySpec {
        
        private IGameplayAbilityDefinition _definition;
        
        private GameplayAbilitySpecHandle _handle;

        private float _level;
        
        private int _inputId;
        
        private bool _isActive;
        
        private IGameplayAbility _instance;
        
        private CancellationTokenSource _cts;
        
        public IGameplayAbilityDefinition Definition => _definition;
        
        public GameplayAbilitySpecHandle Handle => _handle;
        
        public float Level => _level;
        
        public int InputID => _inputId;
        
        public bool IsActive => _isActive;
        
        public CancellationTokenSource Cts => _cts ??= new CancellationTokenSource();

        public IGameplayAbility Instance => _instance ??= Definition.CreateInstance();

        public GameplayAbilitySpec(IGameplayAbilityDefinition def, 
            float level = -1.0f,
            int inputId = -1) {
            _definition = def ?? throw new System.ArgumentNullException();
            _handle = GameplayAbilitySpecHandle.GenerateNewHandle();
            _level = level;
            _inputId = inputId;
            _isActive = false;
        }

        public bool SetAbilityInstance(IGameplayAbility instance) {
            if (_definition.InstancePolicy == GameplayAbilityInstancePolicy.NonInstanced) {
                return false;
            }
            _instance = instance;
            return true;
        }

        public void SetIsActive(bool isActive) {
            _isActive = isActive;
        }
    }
    
    public struct GameplayAbilitySpecHandle {
        private static int _grobalHandle = 0;
        
        private int _handle;

        public static GameplayAbilitySpecHandle GenerateNewHandle() {
            return new GameplayAbilitySpecHandle() {
                _handle = ++_grobalHandle
            };
        }
        
        public static GameplayAbilitySpecHandle InValid => new GameplayAbilitySpecHandle(false);

        public override bool Equals(object obj) {
            return obj is GameplayAbilitySpecHandle other && other._handle == _handle;
        }

        public bool IsValid() => _handle != 0;
        
        public override int GetHashCode() => _handle;
    }
}