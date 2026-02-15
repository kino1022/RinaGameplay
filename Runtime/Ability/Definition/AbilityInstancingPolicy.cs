namespace RinaGameplay.Ability.Definition {
    public enum AbilityInstancingPolicy {
        
        /// <summary>
        /// Abilityはインスタンス化されない。すべてのアクターは同じAbilityクラスのインスタンスを共有する。
        /// </summary>
        NonInstance,
        /// <summary>
        /// AbilityをAbilitySystemComponentを持つアクターごとにインスタンス化する
        /// </summary>
        InstancePerActor,
        /// <summary>
        /// 実行ごとにAbilityをインスタンス化する
        /// </summary>
        InstancePerExecution,
        
    }
}