namespace RinaGameplay.Effect.Definition {
    public enum EffectStackingPeriodPolicy {
        
        /// <summary>
        /// 周期時間はスタックされた際にリセットされない
        /// </summary>
        Never,
        
        /// <summary>
        /// 周期時間はスタックされた際にリセットされる
        /// </summary>
        RefreshOnStack
    }
}