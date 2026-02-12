using System;
using ObservableCollections;

namespace RinaGameplay.Tag.Container {

    public interface IObservableGameplayTagContainer : IGameplayTagContainer {
        
        IReadOnlyObservableList<GameplayTag> ObservableTags { get; }
        
        event Action<GameplayTag> OnTagAdded;
        
        event Action<GameplayTag> OnTagRemoved;
        
    }
    
    public class ObservableGameplayTagContainer {
        
    }
}