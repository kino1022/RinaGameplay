using RinaGameplay.Modifier;

namespace RinaGameplay.Effect {
    public struct GameplayEffectCallback {
        public IGameplayEffectSpec Spec;
        
        public ModifierEvaluatedData EvaluatedData;

        public GameplayAbilitySystem Target;
    }
}