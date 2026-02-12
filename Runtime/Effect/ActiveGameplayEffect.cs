namespace RinaGameplay.Effect {
    
    public interface IActiveGameplayEffect {
        
        IGameplayEffectSpec Spec { get; }
        
        ActiveGameplayEffectHandle Handle { get; }
        
        float StartTime { get; }
        
        float PeriodElapsedTime { get; set; }
        
        void SetHandle (ActiveGameplayEffectHandle handle);
        
        void SetStartTime (float startTime);
        
        /// <summary>
        /// 有効期限が過ぎていないかどうかを確認するメソッド
        /// </summary>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        bool IsExpired (float currentTime);
        
        /// <summary>
        /// DeltaTimeをElapesdTimeに対して加算するメソッド
        /// </summary>
        /// <param name="deltaTime"></param>
        void AddPeriodElapsedTime(float deltaTime);
        
        /// <summary>
        /// 周期的な効果を実行するべきであるかどうかを判断するメソッド
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        bool ShouldExecutePeriodicEffect();
    }
    
    public class ActiveGameplayEffect : IActiveGameplayEffect {

        public ActiveGameplayEffect(IGameplayEffectSpec spec) {
            
        }
    }
    
    public readonly struct ActiveGameplayEffectHandle {

        private static int _grobalHandle = 0;
        
        private readonly int _handle;

        public ActiveGameplayEffectHandle(bool generate) {
            _handle = generate ? ++_grobalHandle : 0;
        }

        public static ActiveGameplayEffectHandle GenerateNewHandle() {
            return new ActiveGameplayEffectHandle(true);
        }
        
        public static ActiveGameplayEffectHandle InValid => new ActiveGameplayEffectHandle(false);

        public override bool Equals(object obj) {
            return obj is ActiveGameplayEffectHandle other && other._handle == _handle;
        }

        public bool IsValid() => _handle != 0;
        
        public override int GetHashCode() => _handle;
    }
}