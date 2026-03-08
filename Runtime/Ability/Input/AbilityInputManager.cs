using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

namespace RinaGameplay.Ability.Input {

    public struct AbilityInputEntry {

        public InputActionReference InputRef { get; }

        public int InputId { get; set; }
        
    }
    
    public class AbilityInputManager : SerializedMonoBehaviour{
        
        private List<AbilityInputEntry> _entries = new List<AbilityInputEntry>();
        
        private AbilitySystemComponent _abilitySystem;

        public void SetEntry(AbilityInputEntry entry) {
            entry.InputRef.action.started += context => OnInputStart(context, ref entry);
            entry.InputRef.action.canceled += context => OnInputEnd(context, ref entry);
            _entries.Add(entry);
        }
        
        private void OnInputStart (InputAction.CallbackContext context, ref AbilityInputEntry entry) {
            var abilities = GetMatchAbilitiesFromEntry(ref entry).ToList();
            if (!abilities.Any()) {
                return;
            }
            foreach (var ability in abilities) {
                _abilitySystem.ActiveAbilities.TryActivateAbility(ability.Handle);
            }
        }
        
        private void OnInputEnd (InputAction.CallbackContext context, ref AbilityInputEntry entry) {
            var abilities = GetMatchAbilitiesFromEntry(ref entry).ToList();
            if (!abilities.Any()) {
                return;
            }
            foreach (var ability in abilities) {
                _abilitySystem.ActiveAbilities.CancelAbility(ability.Handle);
            }
        }

        private IEnumerable<IGameplayAbilitySpec> GetMatchAbilitiesFromEntry(ref AbilityInputEntry entry) {
            var abilities = new List<IGameplayAbilitySpec>();
            foreach (var activeAbility in _abilitySystem.ActiveAbilities.ActiveAbilities) {
                if (activeAbility is null) {
                    continue;
                }
                if (activeAbility.InputID == entry.InputId) {
                    abilities.Add(activeAbility);
                }
            }
            return abilities;
        }
    }
}