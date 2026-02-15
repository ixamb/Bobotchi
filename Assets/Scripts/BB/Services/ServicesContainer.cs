using BB.Services.Missions;
using BB.Services.Modules.Cart;
using BB.Services.Modules.GameData;
using BB.Services.Modules.LocalSave;
using BB.Services.Modules.PlayerPreferences;
using Core.Runtime.Services.Audio;
using Core.Runtime.Services.LocalSave;
using Core.Runtime.Services.Notifications;
using Core.Runtime.Services.Scenes;
using Core.Runtime.Services.Scheduler;
using Core.Runtime.Services.Views;
using UnityEngine;

namespace BB.Services
{
    [RequireComponent(typeof(IActionSchedulerService))]
    [RequireComponent(typeof(IAudioService))]
    [RequireComponent(typeof(ISceneService))]
    [RequireComponent(typeof(IGameDataService))]
    [RequireComponent(typeof(ICartService))]
    [RequireComponent(typeof(ILocalSaveService))]
    [RequireComponent(typeof(IBBLocalSaveService))]
    [RequireComponent(typeof(IViewService))]
    [RequireComponent(typeof(INotificationService))]
    [RequireComponent(typeof(IMissionService))]
    [RequireComponent(typeof(IPlayerPreferenceService))]
    public sealed class ServicesContainer : MonoBehaviour
    {
    }
}