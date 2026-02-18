using RinaGameplay.Animation.Montage.Definition;
using UnityEngine;

namespace RinaGameplay.Animation.Montage {
    [CreateAssetMenu(menuName = "RinaGameplay/Animation/MontageNotify")]
    public class AnimationMontageNotify : ScriptableObject {

        public float TriggerTime = 0.0f;

        public MontageNotifyType NotifyType = MontageNotifyType.Custom;
        
    }
}