using BB.Data;

namespace BB.Services.Modules.LocalSave.Handlers
{
    public sealed class PlayerInformationSaveHandler : SaveHandler<PlayerInformationSaveDto>
    {
        protected override string ConstantEntryKey => Constants.LocalSaveEntries.PlayerInformation;
        
        public void Register(PlayerInformationSaveDto informationSaveDto, bool autoSave = false)
        {
            SetData(informationSaveDto, autoSave);
        }

        public void UpdateSurname(string surname, bool autoSave = false)
        {
            var playerInformation = GetData();
            playerInformation.Surname = surname;
            SetData(playerInformation, autoSave);
        }

        public void UpdateGender(Gender gender, bool autoSave = false)
        {
            var playerInformation = GetData();
            playerInformation.Gender = gender;
            SetData(playerInformation, autoSave);
        }
        
        public bool IsDefault()
        {
            return GetData().IsInitialized();
        }
    }
}