using BB.Entities;
using BB.Management.FurniturePlacement.Props;
using Core.Runtime.Services;

namespace BB.Management.FurniturePlacement
{
    public interface IFurniturePlacementManager : ISingleton
    {
        void Activate();
        void Deactivate();

        void InitializePropPlacementFromInventory(Entities.Prop prop);

        void PlaceProp();
        void RotateProp();
        void DeleteProp(bool retrieveIntoInventory = true);
        void CancelPropPlacement();
        
        void PlaceFloor(Surface surface);
        void PlaceWall(Surface surface);
        
        void RegisterModeObserver(IPropPlacementModeObserver modeObserver);
        void UnregisterModeObserver(IPropPlacementModeObserver modeObserver);

        void RegisterPropSelectedObserver(IPropSelectedObserver selectedObserver);
        void UnregisterPropSelectedObserver(IPropSelectedObserver selectedObserver);

        void RegisterPropUnselectedObserver(IPropUnselectedObserver observer);
        void UnregisterPropUnselectedObserver(IPropUnselectedObserver observer);
        
        void RegisterPlacementObserver(IPropPlacementObserver modeObserver);
        void UnregisterPlacementObserver(IPropPlacementObserver modeObserver);

        void RegisterRemoveObserver(IPropRemoveObserver observer);
        void UnregisterRemoveObserver(IPropRemoveObserver observer);

        void RegisterValidPlacementObserver(IPropPlacementValidObserver observer);
        void UnregisterValidPlacementObserver(IPropPlacementValidObserver observer);

        void RegisterInvalidPlacementObserver(IPropPlacementInvalidObserver observer);
        void UnregisterInvalidPlacementObserver(IPropPlacementInvalidObserver observer);
    }
}