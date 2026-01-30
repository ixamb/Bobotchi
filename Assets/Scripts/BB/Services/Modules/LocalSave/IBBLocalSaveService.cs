using BB.Services.Modules.LocalSave.Handlers;
using Core.Services;

namespace BB.Services.Modules.LocalSave
{
    // ReSharper disable once InconsistentNaming
    public interface IBBLocalSaveService : ISingleton
    {
        void RegisterSetLocalSaveObserver(BBSetLocalSaveObserver observer);
        void UnregisterSetLocalSaveObserver(BBSetLocalSaveObserver observer);
        
        PlayInformationSaveHandler PlayInformation { get; }
        PlayerInformationSaveHandler PlayerInformation { get; }
        BalanceSaveHandler Balance { get; }
        PurchasableEntitySaveHandler PurchasableEntities { get; }
        StateStatSaveHandler StateStat { get; }
        FurniturePlacementSaveHandler FurniturePlacement { get; }
        RunningMissionSaveHandler RunningMission { get; }

        void Save();
        void Delete();
    }
}