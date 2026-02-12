namespace RinaGameplay.Attribute {

    public interface IAttributeSet {

        /// <summary>
        /// IGameplayAttributeに対応するIGameplayAttributeValueを取得する
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        IGameplayAttributeValue GetAttributeValue(IGameplayAttribute attribute);
        
    }
}