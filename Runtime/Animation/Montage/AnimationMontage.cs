using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RinaGameplay.Animation.Montage {

    public interface IAnimationMontage {
        
        /// <summary>
        /// 再生するアニメーション
        /// </summary>
        AnimationClip Clip { get; }
        
        /// <summary>
        /// 再生速度
        /// </summary>
        float PlayRate { get; }
        
        /// <summary>
        /// ブレンドイン時間(秒)
        /// </summary>
        float BlendInTime { get; }
        
        /// <summary>
        /// ブレンドアウト時間(秒)
        /// </summary>
        float BlendOutTime { get; }
        
        /// <summary>
        /// ルートモーションを使用するかどうか
        /// </summary>
        bool UseRootMotion { get; }
        
        /// <summary>
        /// ルートモーションのブレンド倍率
        /// </summary>
        bool RootMotionRate { get; }
        
        /// <summary>
        /// アニメーションの総再生時間(秒)
        /// </summary>
        float Duration { get; }
        
        /// <summary>
        /// アニメーションの区間
        /// </summary>
        IReadOnlyList<AnimationMontageSection> Sections { get; }
        
        /// <summary>
        /// アニメーション再生中に流れる通知
        /// </summary>
        IReadOnlyList<AnimationMontageNotify> Notifies { get; }
        
        IReadOnlyList<AnimationMontageNotifyState> NotifyStates { get; }
        
    }
    [CreateAssetMenu(menuName = "RinaGameplay/Animation/Montage")]
    public class AnimationMontage : SerializedScriptableObject , IAnimationMontage {

        [SerializeField]
        protected AnimationClip _clip;

        [SerializeField]
        protected float _playRate = 1.0f;
        
        [SerializeField]
        protected float _blendInTime = 0.0f;
        
        [SerializeField]
        protected float _blendOutTime = 0.0f;
        
        [SerializeField]
        protected bool _useRootMotion = false;
        
        [SerializeField]
        protected bool _rootMotionRate = false;
        
        [SerializeField]
        protected List<AnimationMontageNotify> _notifies = new List<AnimationMontageNotify>();
        
        [SerializeField]
        protected List<AnimationMontageSection> _sections = new List<AnimationMontageSection>();
        
        [SerializeField]
        protected List<AnimationMontageNotifyState> _notifyStates = new List<AnimationMontageNotifyState>();
        
        public AnimationClip Clip => _clip;
        public float PlayRate => _playRate;
        public float BlendInTime => _blendInTime;
        public float BlendOutTime => _blendOutTime;
        public bool UseRootMotion => _useRootMotion;
        public bool RootMotionRate => _rootMotionRate;
        public float Duration => _clip is not null ? Clip.length / PlayRate : 0.0f;
        public IReadOnlyList<AnimationMontageNotify> Notifies => _notifies;
        public IReadOnlyList<AnimationMontageSection> Sections => _sections;
        public IReadOnlyList<AnimationMontageNotifyState> NotifyStates => _notifyStates;
    }
}