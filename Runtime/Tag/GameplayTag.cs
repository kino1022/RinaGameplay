using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RinaGameplay.Tag {
    public class GameplayTag : SerializedScriptableObject {
        
        [SerializeField]
        private string _name;
        
        [SerializeField]
        private List<GameplayTag> _parentTags = new();

        public string Name => _name;
        
        public IReadOnlyList<GameplayTag> ParentTags => _parentTags;
        
    }
}