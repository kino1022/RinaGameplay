using System.Collections.Generic;
using RinaGameplay.Ability;
using RinaGameplay.Effect.Definition;
using RinaGameplay.Modifier;
using RinaGameplay.Tag.Container;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace RinaGameplay.Effect {

    /// <summary>
    /// エディタ上で定義するGameplayEffectの静的な定義を表現するクラスに対して約束するインターフェース
    /// </summary>
    public interface IGameplayEffect {
        
        /// <summary>
        /// GameplayEffectの持続時間に対する定義
        /// </summary>
        IGameplayEffectDurationDefinition DurationDef { get; }
        
        /// <summary>
        /// GameplayEffectの周期処理に対する定義
        /// </summary>
        IGameplayEffectPeriodicDefinition PeriodicDef { get; }
        
        /// <summary>
        /// GameplayEffectのスタックに対する定義
        /// </summary>
        IGameplayEffectStackingDefinition StackingDef { get; }
        
        /// <summary>
        /// 適用するModifierのリスト
        /// </summary>
        IReadOnlyList<ModifierInfo> Modifiers { get; }
        
        /// <summary>
        /// GameplayEffect中に付与するタグのコンテナ
        /// </summary>
        IGameplayTagContainer GrantTags { get; }
        
        /// <summary>
        /// 適用先のタグの状態によって適用できるかどうかを判断する条件
        /// </summary>
        IGameplayTagRequirements ApplicationRequirements { get; }
        
        /// <summary>
        /// このタグがついている間はGameplayEffectの効果が継続できる
        /// </summary>
        IGameplayTagRequirements OnGoingRequirements { get; }
        
        /// <summary>
        /// 付与されている間にこのタグが付与されると、GameplayEffectが解除される
        /// </summary>
        IGameplayTagRequirements RemovalRequirements { get; }
        
        /// <summary>
        /// GameplayEffect中に付与するGameplayAbilityのリスト
        /// </summary>
        IReadOnlyList<IGameplayAbility> GrantedAbilities { get; }

        /// <summary>
        /// 付与されているタグをもとにしてGameplayEffectを適用できるかどうかを判定する
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        bool CanApply(IGameplayTagContainer container);

    }

    public interface IGameplayEffectDurationDefinition {
        
        /// <summary>
        /// 効果持続ののタイプ
        /// </summary>
        EffectDurationType DurationType { get; }

        /// <summary>
        /// 持続時間を取得する
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        float GetDuration(IGameplayEffectSpec spec);
        
    }

    public interface IGameplayEffectStackingDefinition {
        
        /// <summary>
        /// スタックするかどうかの挙動
        /// </summary>
        EffectStackingPolicy StackingPolicy { get; }
        
        /// <summary>
        /// スタックできる最大数
        /// </summary>
        int MaxStack { get; }
        
        /// <summary>
        /// スタックした際に効果周期をどうするか
        /// </summary>
        EffectStackingPeriodPolicy OnStackPeriod { get; }
        
        /// <summary>
        /// スタックした際に持続時間をどうするか
        /// </summary>
        EffectStackingDurationPolicy OnStackDuration { get; }
        
    }

    public interface IGameplayEffectPeriodicDefinition {
        
        /// <summary>
        /// 周期的に効果を発揮するかどうか
        /// </summary>
        bool ExecutePeriodic { get; }

        /// <summary>
        /// 効果の発動周期を取得する
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        float GetPeriod(IGameplayEffectSpec spec);
        
    }

    public abstract class GameplayEffect : SerializedScriptableObject, IGameplayEffect {
        
        [OdinSerialize]
        protected IGameplayEffectDurationDefinition _durationDef;
        
        [OdinSerialize]
        protected IGameplayEffectPeriodicDefinition _periodDef;
        
        [OdinSerialize]
        protected IGameplayEffectStackingDefinition _stackingDef;
        
        [OdinSerialize]
        protected List<ModifierInfo> _modifiers = new List<ModifierInfo>();
        
        [OdinSerialize]
        protected IGameplayTagContainer _grantTags = new GameplayTagContainer();
        
        [OdinSerialize]
        protected IGameplayTagRequirements _applicationRequirements = new GameplayTagRequirements();

        [OdinSerialize]
        protected IGameplayTagRequirements _onGoingRequirements = new GameplayTagRequirements();
        
        [OdinSerialize]
        protected IGameplayTagRequirements _removalRequirements = new GameplayTagRequirements();
        
        [OdinSerialize]
        protected List<IGameplayAbility> _grantedAbilities = new List<IGameplayAbility>();
        
        public IGameplayEffectDurationDefinition DurationDef => _durationDef;
        
        public IGameplayEffectPeriodicDefinition PeriodicDef => _periodDef;
        
        public IGameplayEffectStackingDefinition StackingDef => _stackingDef;
        
        public IReadOnlyList<ModifierInfo> Modifiers => _modifiers;
        
        public IGameplayTagContainer GrantTags => _grantTags;
        
        public IGameplayTagRequirements ApplicationRequirements => _applicationRequirements;
        
        public IGameplayTagRequirements OnGoingRequirements => _onGoingRequirements;
        
        public IGameplayTagRequirements RemovalRequirements => _removalRequirements;

        public IReadOnlyList<IGameplayAbility> GrantedAbilities => _grantedAbilities;

        public bool CanApply(IGameplayTagContainer container) {
            if (!_applicationRequirements.RequirementMet(container)) {
                return false;
            }
            return true;
        }
    }
}