using RinaGameplay.Animation.Montage.Definition;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RinaGameplay.Animation.Montage {
    [CreateAssetMenu(menuName = "RinaGameplay/Animation/MontageNotifyState")]
    public class AnimationMontageNotifyState : SerializedScriptableObject {
        
        public float StartTime = 0.0f;
        
        public float EndTime = 0.0f;
        
        public MontageStateType StateType = MontageStateType.Custom;
        
    }
}