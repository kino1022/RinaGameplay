using System.Collections.Generic;

namespace RinaGameplay.Tag.Container {
    
    /// <summary>
    /// GameplayTagをリスト化して管理するクラスに対して約束するインターフェース
    /// </summary>
    public interface IGameplayTagContainer {
        
        /// <summary>
        /// 管理しているGameplayTag
        /// </summary>
        IReadOnlyList<GameplayTag> Tags { get; }
        
        /// <summary>
        /// コンテナに対してタグを追加する
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        bool AddTag(GameplayTag tag);
        
        /// <summary>
        /// コンテナから指定のタグを除去する
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        bool RemoveTag(GameplayTag tag);
        
        /// <summary>
        /// コンテナが指定のタグを持っているかどうか判別する
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        bool HasTag(GameplayTag tag);
        
        /// <summary>
        /// コンテナが他のコンテナの持つタグのうち、いずれかを持っているかどうか判別する
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool HasAnyTag (IGameplayTagContainer other);
        
        /// <summary>
        /// コンテナが他のコンテナの持つタグをすべて持っているかどうか判別する
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool HasAllTags (IGameplayTagContainer other);
        
    }
    
    public class GameplayTagContainer {
        
    }
}