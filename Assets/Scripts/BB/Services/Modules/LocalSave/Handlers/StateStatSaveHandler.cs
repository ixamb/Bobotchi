using System.Linq;
using BB.Data;
using UnityEngine;

namespace BB.Services.Modules.LocalSave.Handlers
{
    public sealed class StateStatSaveHandler : SaveHandler<PlayerStatStateSaveDto>
    {
        protected override string ConstantEntryKey => Constants.LocalSaveEntries.CharacterStateStats;
        
        public void Update(CharacterStateStat characterStateStat, float amount, bool autoSave = false)
        {
            var stateStatSaveDto = GetData();
            var stateStatEntry = stateStatSaveDto.StateStatEntries.FirstOrDefault(entry => entry.CharacterStat == characterStateStat);
            if (stateStatEntry is null)
            {
                return;
            }
            
            stateStatEntry.Amount = amount >= 0
                ? Mathf.Min(stateStatEntry.Amount + Mathf.Abs(amount), Constants.ProgressStatStateThresholds.Maximum) 
                : Mathf.Max(stateStatEntry.Amount - Mathf.Abs(amount), Constants.ProgressStatStateThresholds.Minimum);
            
            SetData(stateStatSaveDto, autoSave);
        }
        
        public float Get(CharacterStateStat characterStateStat)
        {
            return GetData()
                .StateStatEntries.FirstOrDefault(entry => entry.CharacterStat == characterStateStat)?.Amount ?? 0;
        }
    }
}