using System.Threading;
using R3;

namespace RinaGameplay.Attribute {

    public interface IGameplayAttributeValue {
        
        IGameplayAttribute Definition { get; }
        
        ReadOnlyReactiveProperty<float> BaseValue { get; }
        
        ReadOnlyReactiveProperty<float> CurrentValue { get; }
        
    }
    
    public class GameplayAttributeValue {
        
    }
}