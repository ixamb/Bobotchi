using System;
using System.Collections;
using Core.Services.Notifications.Dto;
using Unity.Notifications;

namespace Core.Services.Notifications
{
    public interface INotificationService : ISingleton
    {
        IEnumerator RequestPermission(Action<NotificationsPermissionStatus> onRequestReturned);
        void ScheduleNotification(NotificationRequestDto notificationRequest);
        void CancelAllNotifications();
        void OpenNotificationSettings();
    }
}