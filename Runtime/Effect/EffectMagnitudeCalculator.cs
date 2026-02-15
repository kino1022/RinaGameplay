using System;

namespace RinaGameplay.Effect {

    public interface IGameplayEffectMagnitudeCalculator {
                
        /// <summary>
        /// 効果量を計算する
        /// </summary>
        /// <param name="spec">適用するGameplayEffectSpec</param>
        /// <param name="source">効果の発生源のAbilitySystem（攻撃者など）</param>
        /// <param name="target">効果の対象のAbilitySystem（被弾者など）</param>
        /// <returns>計算された効果量</returns>
        float CalculateMagnitude(
            IGameplayEffectSpec spec, 
            AbilitySystemComponent source, 
            AbilitySystemComponent target
        );
    }
    
    [Serializable]
    public struct ScalableFloatMagnitudeCalculator : IGameplayEffectMagnitudeCalculator {
        
        public IScalableFloat ScalableFloat { get; set; }

        public float CalculateMagnitude(
            IGameplayEffectSpec spec, 
            AbilitySystemComponent source, 
            AbilitySystemComponent target
        ) {
            return ScalableFloat.GetValueAtLevel(spec.Level);
        }
    }
    
}