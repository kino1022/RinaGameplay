using System.Collections.Generic;
using R3;
using RinaGameplay.Effect;
using RinaGameplay.Modifier;
using UnityEngine;

namespace RinaGameplay.Attribute {
    public interface IGameplayAttributeValue {
        
        /// <summary>
        /// このGameplayAttributeの定義
        /// </summary>
        IGameplayAttribute Definition { get; }
        
        ReadOnlyReactiveProperty<float> BaseValue { get; }
        
        ReadOnlyReactiveProperty<float> CurrentValue { get; }

        void SetBaseValue(float nextValue);

        void AddModifier(ref ActiveModifier mod);
        
        void RemoveModifier(ref ActiveModifier mod);
        
        void RemoveAllModifiersFromSource(ActiveGameplayEffectHandle sourceEffectHandle);

    }

    public class GameplayAttributeValue : IGameplayAttributeValue {
        
        public IGameplayAttribute Definition { get; private set; }
        
        private ReactiveProperty<float> _baseValue;
        
        private ReactiveProperty<float> _currentValue;
        
        private List<ActiveModifier> _activeModifiers;
        
        public ReadOnlyReactiveProperty<float> BaseValue => _baseValue;
        
        public ReadOnlyReactiveProperty<float> CurrentValue => _currentValue;

        public GameplayAttributeValue(IGameplayAttribute def, float initValue) {
            Definition = def;
            _baseValue = new ReactiveProperty<float>(initValue);
            _currentValue = new ReactiveProperty<float>(initValue);
        }

        public void SetBaseValue(float nextValue) {
            _baseValue.Value = Mathf.Clamp(nextValue, Definition.MinValue, Definition.MaxValue);
            RecalculateCurrentValue();
        }

        public void AddModifier(ref ActiveModifier mod) {
            _activeModifiers.Add(mod);
            _activeModifiers.Sort((a,b) => a.Operator.CompareTo(b.Operator));
            RecalculateCurrentValue();
        }

        public void RemoveModifier(ref ActiveModifier mod) {
            _activeModifiers.Remove(mod);
            RecalculateCurrentValue();
        }

        public void RemoveAllModifiersFromSource(ActiveGameplayEffectHandle sourceEffectHandle) {
            var toRemove = new List<ActiveModifier>();
            _activeModifiers.RemoveAll(x => x.SourceEffectHandle.GetHashCode() == sourceEffectHandle.GetHashCode());
            RecalculateCurrentValue();
        }

        private void RecalculateCurrentValue() {
            var next = _activeModifiers.ApplyActiveModifiers(_baseValue.CurrentValue);
            _currentValue.Value = Mathf.Clamp(next, Definition.MinValue, Definition.MaxValue);
        }
        
    }
}