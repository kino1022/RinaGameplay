using System;
using UnityEngine;

namespace RinaGameplay {

    public interface IScalableFloat {
        /// <summary>
        /// レベルに対応した値を取得する
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        float GetValueAtLevel(float level);
    }
    
    [Serializable]
    public struct ScalableFloat : IScalableFloat {
        
        public float value;

        public AnimationCurve curve;

        public float GetValueAtLevel(float level) {
            if (curve is not null && curve.keys.Length > 0) {
                return value * curve.Evaluate(level);
            }
            return value;
        }
    }
}