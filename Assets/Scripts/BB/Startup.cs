using System;
using System.Threading.Tasks;
using BB.Data;
using BB.Services.Modules.LocalSave;
using Core.Runtime.Services.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BB
{
    public class Startup : MonoBehaviour
    {
        private async void Start()
        {
            try
            {
                await Task.Delay(5000);

                if (BBLocalSaveService.Instance.PlayerInformation.IsDefault())
                {
                    await SceneService.Instance.LoadSceneAsync(Constants.SceneNames.Onboarding, LoadSceneMode.Single);
                }
                else
                {
                    await SceneService.Instance.LoadSceneAsync(Constants.SceneNames.Game, LoadSceneMode.Single);
                }
                
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}