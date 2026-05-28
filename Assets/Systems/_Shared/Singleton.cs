using Systems.Shared.Loggers;
using UnityEngine;

namespace Systems.Shared
{
    public abstract class Singleton<TInstance> : MonoBehaviour where TInstance : MonoBehaviour
    {
        public static TInstance Instance { get; private set; }
        protected static IGameLogger Logger { get; private set; }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InitializeOnLoad()
        {
            Instance = null;
        }

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this as TInstance;
            DontDestroyOnLoad(gameObject);
            Logger ??= GameLogger.GetOrAdd<TInstance>();
        }
    }
}
