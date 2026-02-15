using System;
using System.Collections;
using Core.Runtime.Services.Notifications.Dto;
using Unity.Notifications;

namespace Core.Runtime.Services.Notifications
{
    public interface INotificationService : ISingleton
    {
        IEnumerator RequestPermission(Action<NotificationsPermissionStatus> onRequestReturned);
        void ScheduleNotification(NotificationRequestDto notificationRequest);
        void CancelAllNotifications();
        void OpenNotificationSettings();
    }
}