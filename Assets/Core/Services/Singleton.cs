using UnityEngine;

namespace Core.Services
{
    // ReSharper disable once InconsistentNaming
    public abstract class Singleton<T, IT> : MonoBehaviour, ISingleton
        where T : Singleton<T, IT>
        where IT : class, ISingleton
    {
        private static T _instance;
        private static IT _itInstance;

        public static IT Instance
        {
            get
            {
                if (_itInstance is not null)
                    return _itInstance;
                
                if (_instance is null)
                {
                    _instance = FindAnyObjectByType<T>();
                    if (_instance is null)
                    {
                        Debug.LogError($"Singleton {typeof(T)} not found");
                    }
                }

                _itInstance = _instance as IT;
                if (_itInstance is null)
                {
                    Debug.LogError($"Singleton {typeof(T)} not found");
                }
                return _itInstance;
            }
        }
        
        private void Awake()
        {
            if (_instance is null)
            {
                _instance = this as T;
                DontDestroyOnLoad(transform.root.gameObject);
                Init();
                print($"Singleton {typeof(T)} initialized");
            }
            else
            {
                Destroy(gameObject);
                print ($"Singleton {typeof(T)} destroyed");
            }
        }

        protected abstract void Init();
    }
    
    public interface ISingleton {}
}