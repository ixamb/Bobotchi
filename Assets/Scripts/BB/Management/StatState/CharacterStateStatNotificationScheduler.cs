using System;
using BB.Data;
using BB.Services.Modules.GameData;
using BB.Services.Modules.LocalSave;
using Core.Runtime.Services.Notifications;
using Core.Runtime.Services.Notifications.Dto;
using UnityEngine;

namespace BB.Management.StatState
{
    public class CharacterStateStatNotificationScheduler : MonoBehaviour
    {
        private void OnApplicationQuit()
        {
            ScheduleStateStatNotification(CharacterStateStat.Hunger);
            ScheduleStateStatNotification(CharacterStateStat.Energy);
        }

        private static void ScheduleStateStatNotification(CharacterStateStat stateStat)
        {
            var gameDataOptions = GameDataService.Instance.GameOptions();
            
            var currentStat = BBLocalSaveService.Instance.StateStat.Get(stateStat);
            var pointsToLowStat = currentStat - 20;
            if (pointsToLowStat < 0)
                return;

            var tickRate = stateStat switch
            {
                CharacterStateStat.Hunger => gameDataOptions.HungerPointsDecreasePerTick(),
                CharacterStateStat.Energy => gameDataOptions.EnergyPointsDecreasePerTick(),
                _ => 0
            };
            
            var requiredTicks = pointsToLowStat / tickRate;
            var timeToLowStatInSeconds = requiredTicks * gameDataOptions.TimeBetweenTicksInSeconds();

            NotificationService.Instance.ScheduleNotification(
                new NotificationRequestDto(
                    Title: "Le bobo a faim!",
                    Text: "Viens le nourrir stp.",
                    Schedule: DateTime.Now.AddSeconds(timeToLowStatInSeconds)));
        }
    }
}