using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Services.Scenes
{
    internal sealed class SceneService : Singleton<SceneService, ISceneService>, ISceneService
    {
        private AsyncOperation _currentOperation;

        protected override void Init()
        {
        }

        public async Task LoadSceneAsync(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Additive)
        {
            _currentOperation = SceneManager.LoadSceneAsync(sceneName, loadMode);
            if (_currentOperation == null)
                throw new Exception();

            while (!_currentOperation.isDone)
            {
                await Task.Yield();
            }
        }
        
        public IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadMode, Action onComplete)
        {
            _currentOperation = SceneManager.LoadSceneAsync(sceneName, loadMode);
            yield return new WaitUntil(() => _currentOperation!.isDone);
            onComplete?.Invoke();
        }
        
        public async Task UnloadSceneAsync(string sceneName, UnloadSceneOptions unloadMode = UnloadSceneOptions.None)
        {
            _currentOperation = SceneManager.UnloadSceneAsync(sceneName, unloadMode);
            if (_currentOperation == null)
                throw new Exception();

            while (!_currentOperation.isDone)
            {
                await Task.Yield();
            }
        }
        
        public IEnumerator UnloadSceneAsync(string sceneName, Action onComplete, UnloadSceneOptions unloadMode = UnloadSceneOptions.None)
        {
            _currentOperation = SceneManager.UnloadSceneAsync(sceneName, unloadMode);
            yield return new WaitUntil(() => _currentOperation!.isDone);
            onComplete?.Invoke();
        }
        
        public float GetOperationProgress() => _currentOperation?.progress ?? 0;
    }
}