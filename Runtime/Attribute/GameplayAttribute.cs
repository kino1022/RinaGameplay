using Sirenix.OdinInspector;
using UnityEngine;

namespace RinaGameplay.Attribute {
    public interface IGameplayAttribute {
        
        float MaxValue { get; }
        
        float MinValue { get; }
        
        /// <summary>
        /// 識別用のハッシュ値。
        /// </summary>
        int Hash { get; }
        
    }

    public class GameplayAttribute : SerializedScriptableObject, IGameplayAttribute {

        [SerializeField]
        protected float _minValue = 0.0f;
        
        [SerializeField]
        protected float _maxValue = 9999.0f;
        
        private int? _cacheHash = 0;
        
        public float MinValue => _minValue;
        
        public float MaxValue => _maxValue;

        public int Hash {
            get {
                if (_cacheHash is null) {
                    _cacheHash = GetInstanceID();
                }
                return _cacheHash.Value;
            }
        }
    }
}