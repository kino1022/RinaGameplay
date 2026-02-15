using RinaGameplay.Attribute;
using RinaGameplay.Tag.Container;
using Sirenix.OdinInspector;

namespace RinaGameplay {
    public class AbilitySystemComponent : SerializedMonoBehaviour {
        
        private IAttributeSet _attributeSet;

        private IGameplayTagContainer _tags;

        public IGameplayTagContainer Tags => _tags;
        
        public IAttributeSet AttributeSet => _attributeSet; 
    }
}