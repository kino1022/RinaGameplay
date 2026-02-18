using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace RinaGameplay.Tag.Container {

    /// <summary>
    /// GameplayTagをリスト化して管理するクラスに対して約束するインターフェース
    /// </summary>
    public interface IGameplayTagContainer
    {

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
        bool HasAnyTag(IGameplayTagContainer other);

        /// <summary>
        /// コンテナが他のコンテナの持つタグをすべて持っているかどうか判別する
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool HasAllTags(IGameplayTagContainer other);

    }
    
    [System.Serializable]
    public class GameplayTagContainer : IGameplayTagContainer {

        [OdinSerialize]
        [LabelText("タグ")]
        protected List<GameplayTag> _tags = new();

        protected Dictionary<int, GameplayTag> _IdToTagMap = new();

        public IReadOnlyList<GameplayTag> Tags => _tags;

        public bool AddTag(GameplayTag tag){
            if (HasTag(tag)) return false;
            _tags.Add(tag);
            _IdToTagMap[tag.TagHash] = tag;
            return true;
        }
        
        public bool RemoveTag (GameplayTag tag) {
            if (!HasTag(tag)) return false;
            _tags.Remove(tag);
            _IdToTagMap.Remove(tag.TagHash);
            return true;
        }

        public bool HasTag(GameplayTag tag) {
            return _IdToTagMap.TryGetValue(tag.TagHash, out var _);
        }

        public bool HasAnyTag(IGameplayTagContainer other) {
            foreach (var tag in other.Tags) {
                if (tag is null)
                {
                    continue;
                }
                if (HasTag(tag))
                {
                    return true;
                }
            }
            return false;
        }
        
        public bool HasAllTags (IGameplayTagContainer other) {
            foreach (var tag in other.Tags)
            {
                if (tag is null)
                {
                    continue;
                }
                if (!HasTag(tag))
                {
                    return false;
                }
            }
            return true;
        }
    }
}