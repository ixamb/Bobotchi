using BB.Services.Modules.LocalSave;
using TheForge.Services.Audio;
using TheForge.Services.Delayer;
using TheForge.Services.Notifications;
using TheForge.Services.Scenes;
using TheForge.Services.Scheduler;
using TheForge.Systems.Actions;
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