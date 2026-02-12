namespace RinaGameplay.Tag.Container {
    
    public interface IGameplayTagRequirements {
        
        bool RequirementsMet(IGameplayTagContainer container);
        
    }
    
    public class GameplayTagRequirements : IGameplayTagRequirements {
        
        private IGameplayTagContainer _ignoreTags;
        
        private IGameplayTagContainer _requires;
        
        public bool RequirementsMet(IGameplayTagContainer container) {
            if (_ignoreTags != null && container.HasAnyTags(_ignoreTags)) {
                return false;
            }
            if (_requires != null && !container.HasAllTags(_requires)) {
                return false;
            }
            return true;
        }
    }
}