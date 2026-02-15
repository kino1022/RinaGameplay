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
            return mod.Operator.ApplyOperator(baseValue, mod.EvaluatedMagnitude);
        }
        
        public static float ApplyActiveModifiers (this List<ActiveModifier> mods, float baseValue) {
            float result = baseValue;
            foreach (var mod in mods) {
                if (mod.Operator == GameplayModifierOperator.Additive) {
                    mod.Operator.ApplyOperator(result, mod.EvaluatedMagnitude);
                }
            }

            foreach (var mod in mods) {
                if (mod.Operator == GameplayModifierOperator.Multiplicative) {
                    mod.Operator.ApplyOperator(result, mod.EvaluatedMagnitude);
                }
            }
            
            foreach (var mod in mods) {
                if (mod.Operator == GameplayModifierOperator.Division) {
                    mod.Operator.ApplyOperator(result, mod.EvaluatedMagnitude);
                }
            }
            
            foreach (var mod in mods) {
                if (mod.Operator == GameplayModifierOperator.Override) {
                    mod.Operator.ApplyOperator(result, mod.EvaluatedMagnitude);
                }
            }
            
            return result;
        }
    }
}