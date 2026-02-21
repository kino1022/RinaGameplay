using System.Collections.Generic;
using NUnit.Framework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace RinaGameplay.Attribute {

    public interface IAttributeSet {

        /// <summary>
        /// IGameplayAttributeに対応するIGameplayAttributeValueを取得する
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        IGameplayAttributeValue GetAttributeValue(IGameplayAttribute attribute);

        /// <summary>
        /// IGameplayAttributeに対応するIGameplayAttributeValueを生成してセットする
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="initValue"></param>
        /// <returns></returns>
        IGameplayAttributeValue GiveAttributeValue(IGameplayAttribute attribute, float initValue);

    }

    [System.Serializable]
    public class AttributeSet : IAttributeSet {
        
        [OdinSerialize]
        [LabelText("Attributeのリスト")]
        [ReadOnly]
        private List<IGameplayAttributeValue> _attributes = new();
        
        [OdinSerialize]
        [LabelText("Hash値からAttributeValueを引くための辞書")]
        [ReadOnly]
        private Dictionary<int, IGameplayAttributeValue> _hashToAttributeValueMap = new();

        public IGameplayAttributeValue GetAttributeValue(IGameplayAttribute attribute) {
            if (!_hashToAttributeValueMap.TryGetValue(attribute.Hash, out var value)) {
                return null;
            }
            return value;
        }
        
        public IGameplayAttributeValue GiveAttributeValue(IGameplayAttribute attribute, float initValue) {
            if (_hashToAttributeValueMap.ContainsKey(attribute.Hash)) {
                return null;
            }

            var newValue = new GameplayAttributeValue(attribute, initValue);
            _attributes.Add(newValue);
            _hashToAttributeValueMap[attribute.Hash] = newValue;
            return newValue;
        }
    }
}