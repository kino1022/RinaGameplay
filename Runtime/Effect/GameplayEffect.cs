using System.Collections.Generic;
using RinaGameplay.Modifier;
using RinaGameplay.Tag.Container;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace RinaGameplay.Effect {

    public interface IGameplayEffect {
        
        /// <summary>
        /// 持続の分類
        /// </summary>
        public GameplayEffectDurationType DurationType { get; }

        /// <summary>
        /// レベルごとの持続時間
        /// </summary>
        public ScalableFloat Duration { get; }
        
        /// <summary>
        /// レベルごとの効果発動期回(毒とかスリップとかそういうやつ)
        /// </summary>
        public ScalableFloat Period { get; }
        
        /// <summary>
        /// 周期的に効果を発揮するかどうか
        /// </summary>
        public bool ExecutePeriodicEffect { get; }
        
        /// <summary>
        /// 発動するGameplayModifier
        /// </summary>
        public List<GameplayModifierInfo> Modifiers { get; }
        
        /// <summary>
        /// このGameplayEffect自体のタグ
        /// </summary>
        public IGameplayTagContainer AssetTags { get; }
        
        /// <summary>
        /// GameplayEffectが発動した際に付与されるタグ
        /// </summary>
        public IGameplayTagContainer GrantedTags { get; }
        
        
        public IGameplayTagRequirements ApplicationRequirements { get; }
        
        public IGameplayTagRequirements OnGoingRequirements { get; }
        
        public IGameplayTagRequirements RemovalRequirements { get; }
        
        /// <summary>
        /// 重複発動した際の挙動を決定するタイプ
        /// </summary>
        public GameplayEffectStackingType StackingType { get; }
        
        /// <summary>
        /// 最大スタックカウント
        /// </summary>
        public int MaxStackCount { get; }
        
        /// <summary>
        /// 重複発動した際に持続時間をどうするか決定するタイプ
        /// </summary>
        public GameplayEffectStackingDurationPolicy StackingDurationPolicy { get; }
        
        public GameplayEffectStackingExpirationPolicy StackingExpirationPolicy { get; }

        public bool CanApply(GameplayAbilitySystem asc);
    }
    
    public class GameplayEffect : SerializedScriptableObject {

        protected GameplayEffectDurationType _durationType = GameplayEffectDurationType.Instant;
        
        [Title("Duration")]
        [SerializeField]
        [LabelText("レベルごとの持続時間")]
        protected ScalableFloat _duration = new ScalableFloat();
        
        [Title("Period")]
        [SerializeField]
        [LabelText("レベルごとの処理周期")]
        protected ScalableFloat _period = new ScalableFloat();

        [SerializeField]
        [LabelText("周期的効果を実行するか")]
        protected bool executePeriodicEffect = true;
        
        [Title("Modifiers")]
        
        [OdinSerialize]
        [LabelText("発動するモディファイア")]
        public List<GameplayModifierInfo> modifiers = new List<GameplayModifierInfo>();
        
        public GameplayEffectDurationType DurationType => _durationType;
    }
}