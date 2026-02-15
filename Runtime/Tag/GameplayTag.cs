using Sirenix.OdinInspector;
using UnityEngine;

namespace RinaGameplay.Tag {
    public class GameplayTag : SerializedScriptableObject {

        public GameplayTag ParentTag;
        
        [SerializeField]
        private string tagName;
        
        private int? cachedHashCode;

        public int TagHash {
            get {
                if (!cachedHashCode.HasValue)
                    cachedHashCode = tagName.GetHashCode();
                return cachedHashCode.Value;
            }
        }

    }
}