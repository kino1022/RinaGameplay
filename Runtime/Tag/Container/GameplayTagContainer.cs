using System.Collections.Generic;

namespace RinaGameplay.Tag.Container {

    public interface IGameplayTagContainer {
        
        IReadOnlyList<GameplayTag> Tags { get; }
        
        bool AddTag(GameplayTag tag);
        
        bool RemoveTag(GameplayTag tag);
        
        bool HasTag(GameplayTag tag);
        
        bool HasAllTags(IGameplayTagContainer tags);
        
        bool HasAnyTags(IGameplayTagContainer tags);
        
    }
    
    public class GameplayTagContainer {
        
    }
}