namespace RinaGameplay.Effect {
    public enum GameplayEffectStackingExpirationPolicy {
        ClearEntireStack,
        RemoveSingleStackAndRefreshDuration,
        RefreshDuration,
    }
}