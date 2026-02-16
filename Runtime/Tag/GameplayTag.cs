using System.Collections.Generic;
using RinaGameplay.Tag.Container;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace RinaGameplay.Tag {
    [CreateAssetMenu(menuName = "RinaGameplay/GameplayTag", fileName = "NewGameplayTag")]
    public class GameplayTag : SerializedScriptableObject {

        [OdinSerialize]
        [LabelText("親タグ")]
        [Tooltip("Nullならルートタグになる")]
        private GameplayTag parentTag;
        
        [SerializeField]
        private string tagName = "default_tag";
        
        private int? _cachedHashCode;
        
        private List<GameplayTag> _cachedHierarchy;

        public int TagHash {
            get {
                if (_cachedHashCode is null) {
                    _cachedHashCode = GetInstanceID();
                }
                return _cachedHashCode.Value;
            }
        }

        public IEnumerable<GameplayTag> GetHierarchy() {
            if (_cachedHierarchy is  null) {
                _cachedHierarchy = new List<GameplayTag>();
                var current = this;
                while (current is not null) {
                    _cachedHierarchy.Add(current);
                    current = current.parentTag;
                }
            }
            return _cachedHierarchy;
        }
        
        public bool MatchesTag(GameplayTag other) {
            if (other == null) return false;
            if (this == other) return true; // 参照比較
    
            // 親を辿って判定
            var current = parentTag;
            while (current != null) {
                if (current == other) return true; // 参照比較
                current = current.parentTag;
            }
    
            return false;
        }
    }
}