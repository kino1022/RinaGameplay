using System.Collections.Generic;

namespace RinaGameplay.Tag.Container {
    
    public interface IGameplayTagContainer {
        
        IReadOnlyList<GameplayTag> Tags { get; }
        
        bool AddTag(GameplayTag tag);
        
        bool RemoveTag(GameplayTag tag);
        
        bool HasTag(string tag);
        
        bool HasAnyTag (IGameplayTagContainer other);
        
        bool HasAllTags (IGameplayTagContainer other);
        
    }
    
    public class GameplayTagContainer {
        
    }
}