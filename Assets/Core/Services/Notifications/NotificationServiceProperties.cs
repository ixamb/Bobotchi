using UnityEngine;

namespace Core.Services.Notifications
{
    [CreateAssetMenu(fileName = "Notification Service Properties", menuName = "Core/Services/Notification Service Properties")]
    public class NotificationServiceProperties : ScriptableObject
    {
        [SerializeField] private string androidChannelId = "default";
        [SerializeField] private string androidChannelName = "Notifications";
        [SerializeField] private string androidChannelDescription = "Main notifications";
        
        public string AndroidChannelId => androidChannelId;
        public string AndroidChannelName => androidChannelName;
        public string AndroidChannelDescription => androidChannelDescription;
    }
}