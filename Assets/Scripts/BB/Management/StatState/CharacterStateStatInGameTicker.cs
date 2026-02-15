using System;
using BB.Data;
using BB.Services.Modules.GameData;
using BB.Services.Modules.LocalSave;
using Core.Runtime.Services.Scheduler;
using UnityEngine;

namespace BB.Management.StatState
{
    public sealed class CharacterStateStatInGameTicker : MonoBehaviour
    {
        private IGameDataOptions _gameDataOptions;
        
        private void Start()
        {
            _gameDataOptions = GameDataService.Instance.GameOptions();
            var now = DateTime.Now;
            
            var elapsedSecondsSinceLastClosedApplication = (now - BBLocalSaveService.Instance.PlayInformation.GetLastClosed()).TotalSeconds;
            
            BBLocalSaveService.Instance.StateStat.Update(
                characterStateStat: CharacterStateStat.Hunger, 
                amount: -((float) (elapsedSecondsSinceLastClosedApplication / _gameDataOptions.TimeBetweenTicksInSeconds()) * _gameDataOptions.HungerPointsDecreasePerTick()));
            
            BBLocalSaveService.Instance.StateStat.Update(
                characterStateStat: CharacterStateStat.Energy,
                amount: -((float) (elapsedSecondsSinceLastClosedApplication / _gameDataOptions.TimeBetweenTicksInSeconds()) * _gameDataOptions.EnergyPointsDecreasePerTick()));
            
            BBLocalSaveService.Instance.Save();
            
            ActionSchedulerService.Instance.CreateScheduler(
                code: "hunger-ticker",
                action: () => BBLocalSaveService.Instance.StateStat.Update(CharacterStateStat.Hunger, -_gameDataOptions.HungerPointsDecreasePerTick(), autoSave: true),
                durationInSeconds: _gameDataOptions.TimeBetweenTicksInSeconds(),
                SchedulerEndAction.Repeat);
            
            ActionSchedulerService.Instance.CreateScheduler(
                code: "energy-ticker",
                action: () => BBLocalSaveService.Instance.StateStat.Update(CharacterStateStat.Energy, -_gameDataOptions.EnergyPointsDecreasePerTick(), autoSave: true),
                durationInSeconds: _gameDataOptions.TimeBetweenTicksInSeconds(),
                SchedulerEndAction.Repeat);
        }

        private void OnApplicationQuit()
        {
            BBLocalSaveService.Instance.PlayInformation.UpdateLastClosed(DateTime.Now, autoSave: true);
        }
    }
}