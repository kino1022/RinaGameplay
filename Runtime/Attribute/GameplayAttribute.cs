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
        
        void OnPostBaseValueChanged (float oldValue, float newValue);
        
        float OnPreBaseValueChanged (float oldValue, float newValue);

        float OnPreBaseValueChangeFromModifier(float oldValue, float nextValue, IGameplayAttributeValue attributeValue);
        
        void OnPostBaseValueChangeFromModifier(float oldValue, IGameplayAttributeValue attributeValue);

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
        
        public virtual void OnPostBaseValueChanged(float oldValue, float newValue) {
            // デフォルトでは何もしない
        }
        
        public virtual float OnPreBaseValueChanged(float oldValue, float newValue) {
            // デフォルトでは変更をそのまま許可する
            return newValue;
        }
        
        public virtual float OnPreBaseValueChangeFromModifier(float oldValue, float nextValue, IGameplayAttributeValue attributeValue) {
            // デフォルトでは変更をそのまま許可する
            return nextValue;
        }

        public virtual void OnPostBaseValueChangeFromModifier(float oldValue, IGameplayAttributeValue attributeValue) {
            // デフォルトでは何もしない
        }
    }
}