using System;
using RinaGameplay.Attribute;
using RinaGameplay.Effect;
using RinaGameplay.Tag.Container;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace RinaGameplay.Modifier {
    public class GameplayModifier : SerializedScriptableObject {
        
    }

    [Serializable]
    public class GameplayModifierInfo {

        public GameplayAttribute attribute;

        public GameplayModifierOperator operation;

        public GameplayEffectMagnitude magnitude;

        [OdinSerialize]
        public IGameplayTagRequirements sourceTags;
        
        [OdinSerialize]
        public IGameplayTagRequirements targetTags;
        
    }
    
    [Serializable]
    public struct ScalableFloat {

        public float Value;

        public AnimationCurve Curve;
        
        public float GetValueAtLevel(float level) {
            if (Curve is not null && Curve.keys.Length > 0) {
                return Value * Curve.Evaluate(level);
            }

            return Value;
        }
    }
}