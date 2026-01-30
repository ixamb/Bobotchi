using BB.Services.Modules.LocalSave;
using Core.Services.Audio;
using Core.Services.Delayer;
using Core.Services.Notifications;
using Core.Services.Scenes;
using Core.Services.Scheduler;
using Core.Systems.Actions;
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