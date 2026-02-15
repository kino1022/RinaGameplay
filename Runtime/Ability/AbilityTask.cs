using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace RinaGameplay.Ability {

    public interface IAbilityTask {

        UniTask StartAbilityTask(CancellationToken token, IActiveGameplayAbility activeAbility);
        
    }

    public interface IActiveAbilityTaskContainer {
        
        IReadOnlyList<IAbilityTask> Tasks { get; }
        
        bool AddTask(IAbilityTask task);
        
        bool RemoveTask(IAbilityTask task);
        
        bool RemoveAllTasks();
        
        void ActivateAllTask(IActiveGameplayAbility activeAbility);
        
        void ActivateTask(IAbilityTask task, IActiveGameplayAbility activeAbility);
        
        void CancelAllTasks();
        
        void CancelTask(IAbilityTask task);
        
    }
    
}