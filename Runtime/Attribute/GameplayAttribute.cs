namespace RinaGameplay.Attribute {
    public interface IGameplayAttribute {
        
        float MaxValue { get; }
        
        float MinValue { get; }
        
        /// <summary>
        /// 識別用のハッシュ値。
        /// </summary>
        int Hash { get; }
        
    }
}