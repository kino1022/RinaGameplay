using RinaGameplay.Effect;

namespace RinaGameplay.Attribute {

    public interface IGameplayAttribute {
        
        void PreAttributeChange(IGameplayAttributeValue value, ref float nextValue);

        void PostAttributeChange(IGameplayAttributeValue value, float oldValue, float nextValue);
        
        void PostGameplayEffectExecute(IGameplayAttributeValue value, GameplayEffectCallback callback);
        
    }
    
    public class GameplayAttribute {
        
    }
}