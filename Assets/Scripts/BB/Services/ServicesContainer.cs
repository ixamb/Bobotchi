using BB.Services.Missions;
using BB.Services.Modules.Cart;
using BB.Services.Modules.GameData;
using BB.Services.Modules.LocalSave;
using BB.Services.Modules.PlayerPreferences;
using Core.Services.Audio;
using Core.Services.LocalSave;
using Core.Services.Notifications;
using Core.Services.Scenes;
using Core.Services.Scheduler;
using Core.Services.Views;
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