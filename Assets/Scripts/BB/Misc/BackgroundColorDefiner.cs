using BB.Services.Modules.PlayerPreferences;
using BB.Services.Modules.PlayerPreferences.Handlers;
using UnityEngine;

namespace BB.Misc
{
    public class BackgroundColorDefiner : MonoBehaviour, IBackgroundColorHandlerObserver
    {
        [SerializeField] private new Camera camera;

        private void Awake()
        {
            PlayerPreferenceService.Instance.BackgroundColor.RegisterObserver(this);
            UpdateCameraBackgroundColor(PlayerPreferenceService.Instance.BackgroundColor.ActiveBackgroundColor());
        }

        public void OnBackgroundColorChanged(Color color)
        {
            UpdateCameraBackgroundColor(color);
        }
        
        private void UpdateCameraBackgroundColor(Color color) => camera.backgroundColor = color;
    }
}