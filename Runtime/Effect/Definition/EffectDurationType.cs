namespace RinaGameplay.Effect.Definition {
    /// <summary>
    /// GameplayEffectの効果の持続の分類を示す列挙型
    /// </summary>
    public enum EffectDurationType {
        
        /// <summary>
        /// 削除されない限り無限に持続する
        /// </summary>
        Infinite,
        
        /// <summary>
        /// 指定された時間の間持続する
        /// </summary>
        Duration,
        
        /// <summary>
        /// 一度効果を発揮して即座に終了する
        /// </summary>
        Instant,
        
    }
}