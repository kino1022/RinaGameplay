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
        
    }

    public static class ActiveModifierExtensions {
        
        public static float ApplyModifierValue (this ActiveModifier mod, float baseValue) {
            if (mod.Operator ==　GameplayModifierOperator.Additive) {
                return baseValue + mod.EvaluatedMagnitude;
            }
            else if (mod.Operator == GameplayModifierOperator.Multiplicative) {
                return baseValue * mod.EvaluatedMagnitude;
            }
            else if (mod.Operator == GameplayModifierOperator.Division) {
                return baseValue / mod.EvaluatedMagnitude;
            }
            else if (mod.Operator == GameplayModifierOperator.Override) {
                return mod.EvaluatedMagnitude;
            }

            return baseValue;
        }
        
        public static float ApplyActiveModifiers (this List<ActiveModifier> mods, float baseValue) {
            float result = baseValue;
            foreach (var mod in mods) {
                if (mod.Operator == GameplayModifierOperator.Additive) {
                    result += mod.EvaluatedMagnitude;
                }
            }

            foreach (var mod in mods) {
                if (mod.Operator == GameplayModifierOperator.Multiplicative) {
                    result *= mod.EvaluatedMagnitude;
                }
            }
            
            foreach (var mod in mods) {
                if (mod.Operator == GameplayModifierOperator.Division) {
                    result /= mod.EvaluatedMagnitude;
                }
            }
            
            foreach (var mod in mods) {
                if (mod.Operator == GameplayModifierOperator.Override) {
                    result = mod.EvaluatedMagnitude;
                }
            }
            
            return result;
        }
    }
}