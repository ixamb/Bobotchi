using UnityEngine;

namespace BB
{
    public static class ServiceLoader
    {
        private const string Path = "Core/__SERVICESCONTAINER__";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadServiceContainer()
        {
            var serviceContainer = Resources.Load(Path);
            Object.Instantiate(serviceContainer);
            Object.DontDestroyOnLoad(serviceContainer);
        }
    }
}