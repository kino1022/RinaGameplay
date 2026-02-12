using RinaGameplay.Attribute;

namespace RinaGameplay.Modifier {
    public struct EvaluatedModifier {
        
        public GameplayAttribute Attribute { get; }
        
        public GameplayModifierOperator Operator { get; }
        
        public float Magnitude { get; }
        
    }
}