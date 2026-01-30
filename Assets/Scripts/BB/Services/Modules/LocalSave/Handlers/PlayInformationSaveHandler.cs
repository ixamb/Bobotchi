using System;
using BB.Data;

namespace BB.Services.Modules.LocalSave.Handlers
{
    public sealed class PlayInformationSaveHandler : SaveHandler<PlayInformationSaveDto>
    {
        protected override string ConstantEntryKey => Constants.LocalSaveEntries.PlayInformation;

        public DateTime GetLastClosed()
        {
            return GetData().LastClosed;
        }
        
        public void UpdateLastClosed(DateTime lastClosed, bool autoSave = false)
        {
            var playInformationDto = GetData();
            playInformationDto.LastClosed = lastClosed;
            SetData(playInformationDto, autoSave);
        }
    }
}