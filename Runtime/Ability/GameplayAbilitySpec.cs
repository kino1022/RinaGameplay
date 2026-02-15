namespace RinaGameplay.Ability {

    public interface IGameplayAbilitySpec {
        
        IGameplayAbility Definition { get; }

        GameplayAbilitySpecHandle Handle { get; }
        
        int Level { get; set; }
        
        int InputID { get; set; }
        
        bool IsActive { get; set; }
        
        IActiveGameplayAbility AbilityInstance { get; set; }

    }
    
    public readonly struct GameplayAbilitySpecHandle {
        
        private static int _grobalHandle = 0;

        private readonly int _handle;

        private GameplayAbilitySpecHandle(bool generate) {
            _handle = generate ? ++_grobalHandle : 0;
        }

        public static GameplayAbilitySpecHandle InValid => new GameplayAbilitySpecHandle();
        
        public static GameplayAbilitySpecHandle GenerateNewHandle() => new GameplayAbilitySpecHandle();
        
        public bool IsValid => _handle != 0;

        public override int GetHashCode() => _handle;
        
    }
    
    public class GameplayAbilitySpec : IGameplayAbilitySpec{
        
        public IGameplayAbility Definition { get; }
        
        public GameplayAbilitySpecHandle Handle { get; }
        
        public int Level { get; set; }
        
        public int InputID { get; set; }
        
        public bool IsActive { get; set; }
        
        public IActiveGameplayAbility AbilityInstance { get; set; }
        
        public GameplayAbilitySpec (IGameplayAbility definition, int level = 1, int inputID = -1) {
            Definition = definition;
            Handle = GameplayAbilitySpecHandle.GenerateNewHandle();
            Level = level;
            InputID = inputID;
        }
    }
}