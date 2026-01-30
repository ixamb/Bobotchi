using System;

namespace Core.Services.Notifications.Dto
{
    public sealed record NotificationRequestDto (string Title, string Text, DateTime Schedule)
    {
        public string Title { get; } = Title;
        public string Text { get; } = Text;
        public DateTime Schedule { get; } = Schedule;
    }
}