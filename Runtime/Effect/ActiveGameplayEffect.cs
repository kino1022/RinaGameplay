namespace RinaGameplay.Effect {
    public interface IActiveGameplayEffect {
        
        ActiveGameplayEffectHandle Handle { get; }
    }

    public readonly struct ActiveGameplayEffectHandle {

        private static int _grobalHandle = 0;

        private readonly int _handle;

        private ActiveGameplayEffectHandle(bool generate) {
            _handle = generate ? ++_grobalHandle : 0;
        }

        public static ActiveGameplayEffectHandle InValid => new ActiveGameplayEffectHandle(false);
        
        public static ActiveGameplayEffectHandle GenerateNewHandle() => new ActiveGameplayEffectHandle(true);
        
        public bool IsValid => _handle != 0;

        public override int GetHashCode() => _handle;
    }
}