using System.Collections.Generic;
using RinaGameplay.Attribute;
using RinaGameplay.Effect;
using RinaGameplay.Modifier.Definition;

namespace RinaGameplay.Modifier {
    public readonly struct ActiveModifier {

        public readonly IGameplayAttribute Target;

        public readonly GameplayModifierOperator Operator;
        
        public readonly float EvaluatedMagnitude;
        
        public readonly ActiveGameplayEffectHandle SourceEffectHandle;
        
        public ActiveModifier(IGameplayAttribute target, GameplayModifierOperator op, float magnitude, ActiveGameplayEffectHandle sourceEffectHandle) {
            Target = target;
            Operator = op;
            EvaluatedMagnitude = magnitude;
            SourceEffectHandle = sourceEffectHandle;
        }
        
    }

    public static class ActiveModifierExtensions {
        
        public static float ApplyModifierValue (this ActiveModifier mod, float baseValue) {
            return mod.Operator.ApplyOperator(baseValue, mod.EvaluatedMagnitude);
        }
        
        public static float ApplyActiveModifiers (this List<ActiveModifier> mods, float baseValue) {
            float result = baseValue;
            foreach (var mod in mods) {
                if (mod.Operator == GameplayModifierOperator.Additive) {
                    result += mod.Operator.ApplyOperator(result, mod.EvaluatedMagnitude);
                }
            }

            foreach (var mod in mods) {
                if (mod.Operator == GameplayModifierOperator.Multiplicative) {
                    result += mod.Operator.ApplyOperator(result, mod.EvaluatedMagnitude);
                }
            }
            
            foreach (var mod in mods) {
                if (mod.Operator == GameplayModifierOperator.Division) {
                    result += mod.Operator.ApplyOperator(result, mod.EvaluatedMagnitude);
                }
            }
            
            foreach (var mod in mods) {
                if (mod.Operator == GameplayModifierOperator.Override) {
                    result += mod.Operator.ApplyOperator(result, mod.EvaluatedMagnitude);
                }
            }
            
            return result;
        }
    }
}