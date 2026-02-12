using System.Collections.Generic;
using RinaGameplay.Ability;
using RinaGameplay.Effect.Definition;
using RinaGameplay.Tag.Container;

namespace RinaGameplay.Effect {

    /// <summary>
    /// エディタ上で定義するGameplayEffectの静的な定義を表現するクラスに対して約束するインターフェース
    /// </summary>
    public interface IGameplayEffect {
        
        /// <summary>
        /// GameplayEffect中に付与するタグのコンテナ
        /// </summary>
        IGameplayTagContainer GrantTags { get; }
        
        IGameplayTagRequirements ApplicationRequirements { get; }
        
        IGameplayTagRequirements OnGoingRequirements { get; }
        
        IGameplayTagRequirements RemovalRequirements { get; }
        
        /// <summary>
        /// GameplayEffect中に付与するGameplayAbilityのリスト
        /// </summary>
        IReadOnlyList<IGameplayAbility> GrantedAbilities { get; }
        
    }
    
}