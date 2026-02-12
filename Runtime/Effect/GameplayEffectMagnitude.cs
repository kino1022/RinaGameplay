using RinaGameplay.Modifier;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RinaGameplay.Effect {
    [System.Serializable]
    public class GameplayEffectMagnitude {
        
        [SerializeField]
        [LabelText("計算方法")]
        private GameplayEffectMagnitudeCalculation calculationType;
        
        [SerializeField]
        [LabelText("Scalable Float")]
        private ScalableFloat _scalableFloat = new ScalableFloat();
        
        
        
        public GameplayEffectMagnitudeCalculation CalculationType => calculationType;
        
        public ScalableFloat ScalableFloat => _scalableFloat;
    }
}