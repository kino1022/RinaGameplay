using Sirenix.Serialization;

namespace RinaGameplay.Tag.Container {

    public interface IGameplayTagRequirements
    {

        bool RequirementMet(IGameplayTagContainer tagContainer);

    }
    
    
    [System.Serializable]
    public class GameplayTagRequirements : IGameplayTagRequirements {

        [OdinSerialize]
        private IGameplayTagContainer _requireTags;
        
        [OdinSerialize]
        private IGameplayTagContainer _ignoreTags;
        
        public bool RequirementMet(IGameplayTagContainer tagContainer) {
            return
            _requireTags != null && tagContainer.HasAllTags(_requireTags) &&
            _ignoreTags != null && !tagContainer.HasAnyTag(_ignoreTags);
        }
    }
}