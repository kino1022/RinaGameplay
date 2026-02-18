using Sirenix.OdinInspector;
using UnityEngine;

namespace RinaGameplay.Animation.Montage {
    [CreateAssetMenu(menuName = "RinaGameplay/Animation/MontageSection")]
    public class AnimationMontageSection : ScriptableObject {
        
        [SerializeField]
        [Tooltip("セクションの名前")]
        public string sectionName = "Default";

        [SerializeField]
        [Tooltip("セクションの開始時間")]
        public float startTime = 0.0f;

        [SerializeField]
        [Tooltip("次のセクション")]
        public string nextSection = "";

    }
}