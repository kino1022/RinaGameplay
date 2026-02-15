using RinaGameplay.Attribute;
using RinaGameplay.Modifier.Definition;

namespace RinaGameplay.Modifier {
    /// <summary>
    /// 適用済みのModifierを表す構造体
    /// </summary>
    public struct EvaluatedModifier {
        public IGameplayAttribute Attribute;
        public GameplayModifierOperator Operator;
        public float Magnitude;
    }
}