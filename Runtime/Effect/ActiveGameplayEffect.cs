using System.Collections.Generic;
using RinaGameplay.Effect.Definition;
using RinaGameplay.Modifier;
using UnityEngine;

namespace RinaGameplay.Effect {
    public interface IActiveGameplayEffect {
        
        IGameplayEffectSpec Spec { get; }
        
        ActiveGameplayEffectHandle Handle { get; }
        
        float StartTime { get; }
        
        float PeriodElapsed { get; }
        
        List<ActiveModifier> AppliedModifiers { get; }
        
        void SetStartTime(float time);
        
        void SetPeriodElapsed(float elapsed);
        
    }

    public readonly struct ActiveGameplayEffectHandle {

        private static int _grobalHandle = 0;

        private readonly int _handle;

        private ActiveGameplayEffectHandle(bool generate) {
            _handle = generate ? ++_grobalHandle : 0;
        }

        public static ActiveGameplayEffectHandle InValid => new ActiveGameplayEffectHandle(false);
        
        public static ActiveGameplayEffectHandle GenerateNewHandle() => new ActiveGameplayEffectHandle(true);
        
        public bool IsValid => _handle != 0;

        public override int GetHashCode() => _handle;
    }

    public class ActiveGameplayEffect : IActiveGameplayEffect {
        
        private IGameplayEffectSpec _spec;
        
        private ActiveGameplayEffectHandle _handle;
        
        private float _startTime = 0.0f;
        
        private float _periodElapsed = 0.0f;
        
        private List<ActiveModifier> _appliedModifiers = new List<ActiveModifier>();
        
        public IGameplayEffectSpec Spec => _spec;
        
        public ActiveGameplayEffectHandle Handle => _handle;
        
        public float StartTime => _startTime;
        
        public float PeriodElapsed => _periodElapsed;
        
        public List<ActiveModifier> AppliedModifiers => _appliedModifiers;

        public ActiveGameplayEffect(IGameplayEffectSpec spec) {
            _spec = spec;
            _handle = ActiveGameplayEffectHandle.GenerateNewHandle();
        }

        public void SetStartTime(float time) {
            _startTime = time;
        }

        public void SetPeriodElapsed(float elapsed) {
            _periodElapsed = elapsed;
        }
    }

    public static class ActiveGameplayEffectExtensions {

        /// <summary>
        /// 周期的効果を発動するべきであるかどうかを取得する
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        public static bool ShouldExecutePeriodicEffect(this IActiveGameplayEffect effect, float currentTime) {
            if (!effect.Spec.Definition.PeriodicDef.ExecutePeriodic || effect.Spec.Period <= 0.0f ) 
                return false;
            if (effect.PeriodElapsed >= effect.Spec.Period) {
                effect.SetPeriodElapsed(effect.PeriodElapsed - effect.Spec.Period);
                return true;
            }

            return false;
        }

        public static bool IsExpired(this IActiveGameplayEffect effect, float currentTime) {
            if (effect.Spec.Definition.DurationDef.DurationType != EffectDurationType.Duration) {
                return false;
            }
            return (currentTime - effect.StartTime) >= effect.Spec.Duration;
        }
    }
}