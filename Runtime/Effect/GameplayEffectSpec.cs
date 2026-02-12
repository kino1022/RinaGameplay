using System.Collections.Generic;
using RinaGameplay.Modifier;
using RinaGameplay.Tag;
using UnityEngine;

namespace RinaGameplay.Effect {
    
    public interface IGameplayEffectSpec {
        
        IGameplayEffect Definition { get;  }
        
        float Level { get;  }
        
        float Duration { get; }
        
        float Period { get;  }
        
        int StackCount { get;  }
        
        IGameplayEffectContextHandle ContextHandle { get; }
        
        Dictionary<GameplayTag, float> SetByCallerData { get;  }
        
        IReadOnlyList<EvaluatedModifier> EvaluatedModifiers { get; }
        
        void SetSetByCallerMagnitude (GameplayTag tag, float magnitude);
        
        void GetSetByCallerMagnitude (GameplayTag tag, float defaultValue = 0.0f);

        void RecalculateModifiers(GameplayAbilitySystem target);

    }

    public interface IGameplayEffectContextHandle {
        
        GameObject SourceObject { get; }
        
        GameObject TargetObject { get; }
        
        GameplayAbilitySystem SourceAbilitySystem { get; }
        
        GameplayAbilitySystem TargetAbilitySystem { get; }
        
    }
    
    [System.Serializable]
    public class GameplayEffectSpec {
        
    }
}