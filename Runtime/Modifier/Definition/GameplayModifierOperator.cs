namespace RinaGameplay.Modifier.Definition {
    public enum GameplayModifierOperator {
        
        /// <summary>
        /// 値に対して加算する
        /// </summary>
        Additive,
        
        /// <summary>
        /// 乗算で計算する
        /// </summary>
        Multiplicative,
        
        /// <summary>
        /// 除算で計算する
        /// </summary>
        Division,
        
        /// <summary>
        /// 値をModifierの値で上書きする
        /// </summary>
        Override,
    }

    public static class GameplayModifierOperatorExtensions {
        
        public static float ApplyOperator (this GameplayModifierOperator op, float baseValue, float modValue) {
            if (op == GameplayModifierOperator.Additive) {
                return baseValue + modValue;
            }
            else if (op == GameplayModifierOperator.Multiplicative) {
                return baseValue * modValue;
            }
            else if (op == GameplayModifierOperator.Division) {
                return baseValue / modValue;
            }
            else if (op == GameplayModifierOperator.Override) {
                return modValue;
            }

            return baseValue;
        }
    }
}