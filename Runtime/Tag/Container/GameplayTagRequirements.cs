namespace RinaGameplay.Tag.Container {

    public interface IGameplayTagRequirements {
        
        bool RequirementMet(IGameplayTagContainer tagContainer);
        
    }
    
    public class GameplayTagRequirements {
        
        private IGameplayTagContainer _requireTags;
        
        private IGameplayTagContainer _ignoreTags;
        
        public bool AreRequirementsMet(IGameplayTagContainer tagContainer) {
            return _requireTags != null && tagContainer.HasAllTags(_requireTags) &&
                   _ignoreTags != null && !tagContainer.HasAnyTag(_ignoreTags);
        }
    }
}