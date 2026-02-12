using RinaGameplay.Attribute;

namespace RinaGameplay.Modifier {
    public struct ModifierEvaluatedData {
        public IGameplayAttributeValue Attribute;
        public GameplayModifierOperator Operator;
        public float Magnitude;
    }
}