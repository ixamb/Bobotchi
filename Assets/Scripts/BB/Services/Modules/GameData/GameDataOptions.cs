using BB.Data;
using UnityEngine;

namespace BB.Services.Modules.GameData
{
    public interface IGameDataOptions
    {
        float TimeBetweenTicksInSeconds();
        float HungerPointsDecreasePerTick();
        float EnergyPointsDecreasePerTick();
        float LowStateStatThreshold();
    }
    
    [CreateAssetMenu(fileName = "Game Data Options", menuName = "BB/Modules/GameData/Game Data Options")]
    public sealed class GameDataOptions : ScriptableObject, IGameDataOptions
    {
        [Header("Stat ticks")]
        [SerializeField] private float timeBetweenTicksInSeconds = 30f;
        [SerializeField] private float hungerPointsDecreasePerTick = 0.125f;
        [SerializeField] private float energyPointsDecreasePerTick = 0.1f;
        
        [SerializeField][Range(0, Constants.ProgressStatStateThresholds.Maximum)]
        private float lowStateStatThreshold = 20f;
        
        public float TimeBetweenTicksInSeconds() => timeBetweenTicksInSeconds;
        public float HungerPointsDecreasePerTick() => hungerPointsDecreasePerTick;
        public float EnergyPointsDecreasePerTick() => energyPointsDecreasePerTick;
        public float LowStateStatThreshold() => lowStateStatThreshold;
    }
}