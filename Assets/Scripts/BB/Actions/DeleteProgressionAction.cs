using BB.Services.Modules.LocalSave;
using Core.Runtime.Services.Audio;
using Core.Runtime.Services.Delayer;
using Core.Runtime.Services.Notifications;
using Core.Runtime.Services.Scenes;
using Core.Runtime.Services.Scheduler;
using Core.Runtime.Systems.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BB.Actions
{
    [CreateAssetMenu(fileName = "Currency Effect Action", menuName = "BB/Actions/Currency Effect Action")]
    public class DeleteProgressionAction : GameAction
    {
        protected override void Executable()
        {
            BBLocalSaveService.Instance.Delete();
            NotificationService.Instance.CancelAllNotifications();
            AudioService.Instance.StopAllAudios();
            ActionSchedulerService.Instance.DestroyAllSchedulers();
            ActionDelayerService.Instance.CancelAll();
            SceneService.Instance.LoadSceneAsync("Onboarding Scene", LoadSceneMode.Single);
        }
    }
}