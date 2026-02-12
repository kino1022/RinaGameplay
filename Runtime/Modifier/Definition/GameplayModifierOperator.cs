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
}