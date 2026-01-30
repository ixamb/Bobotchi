using System;
using System.Linq;
using BB.Data;

namespace BB.Services.Modules.LocalSave.Handlers
{
    public class BalanceSaveHandler : SaveHandler<BalanceSaveDto>
    {
        protected override string ConstantEntryKey => Constants.LocalSaveEntries.CharacterBalances;

        public void Update(Currency currency, float amount, bool autoSave = false)
        {
            var balanceSaveDto = GetData();
            var balanceEntryDto = balanceSaveDto.Balances.FirstOrDefault(balance => balance.Currency == currency);
            if (balanceEntryDto is null)
                return;
            
            balanceEntryDto.Amount = amount >= 0
                ? (float) Math.Round(balanceEntryDto.Amount + amount, 2)
                : (float) Math.Round(balanceEntryDto.Amount - amount, 2);
            
            SetData(balanceSaveDto);
        }
        
        public float Get(Currency currency)
        {
            return GetData().Balances.FirstOrDefault(balance => balance.Currency == currency)!.Amount;
        }
    }
}