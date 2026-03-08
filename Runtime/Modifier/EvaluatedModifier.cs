using RinaGameplay.Attribute;
using RinaGameplay.Modifier.Definition;

namespace RinaGameplay.Modifier {
    /// <summary>
    /// 適用済みのModifierを表す構造体
    /// </summary>
    public struct EvaluatedModifier {
        /// <summary>
        /// 適用対象のGameplayAttribute
        /// </summary>
        public IGameplayAttribute Attribute;
        /// <summary>
        /// 適用されたModifierの演算方法
        /// </summary>
        public GameplayModifierOperator Operator;
        /// <summary>
        /// 適用した効果量
        /// </summary>
        public float Magnitude;
    }
}