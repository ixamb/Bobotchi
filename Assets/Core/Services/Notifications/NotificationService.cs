using System;
using System.Collections;
using Core.Services.Notifications.Dto;
using Unity.Notifications;
using UnityEngine;

namespace Core.Services.Notifications
{
    public sealed class NotificationService : Singleton<NotificationService, INotificationService>, INotificationService
    {
        [SerializeField] private NotificationServiceProperties notificationServiceProperties;
        
        protected override void Init()
        {
            var args = NotificationCenterArgs.Default;
            args.AndroidChannelId = notificationServiceProperties.AndroidChannelId;
            args.AndroidChannelName = notificationServiceProperties.AndroidChannelName;
            args.AndroidChannelDescription = notificationServiceProperties.AndroidChannelDescription;
            NotificationCenter.Initialize(args);
        }

        public IEnumerator RequestPermission(Action<NotificationsPermissionStatus> onRequestReturned)
        {
            var request = NotificationCenter.RequestPermission();
            if (request.Status == NotificationsPermissionStatus.RequestPending)
                yield return request;
            onRequestReturned?.Invoke(request.Status);
        }

        public void ScheduleNotification(NotificationRequestDto notificationRequest)
        {
            var notification = new Notification
            {
                Title = notificationRequest.Title,
                Text = notificationRequest.Text,
            };
            
            var when = new NotificationDateTimeSchedule(notificationRequest.Schedule);
            NotificationCenter.ScheduleNotification(notification, when);
        }

        public void CancelAllNotifications()
            => NotificationCenter.CancelAllScheduledNotifications();
        
        public void OpenNotificationSettings()
            => NotificationCenter.OpenNotificationSettings();
    }
}