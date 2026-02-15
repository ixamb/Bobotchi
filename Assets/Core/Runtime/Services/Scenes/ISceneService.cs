using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.Runtime.Services.Scenes
{
    public interface ISceneService : ISingleton
    {
        Task LoadSceneAsync(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Additive);
        IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadMode, Action onComplete);
        Task UnloadSceneAsync(string sceneName, UnloadSceneOptions unloadMode = UnloadSceneOptions.None);
        IEnumerator UnloadSceneAsync(string sceneName, Action onComplete, UnloadSceneOptions unloadMode = UnloadSceneOptions.None);
        float GetOperationProgress();

    }
}