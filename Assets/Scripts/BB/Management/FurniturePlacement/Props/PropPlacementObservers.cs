using BB.Grid.Tiles;

namespace BB.Management.FurniturePlacement.Props
{
    public interface IPropPlacementModeObserver
    {
        void OnPlacementModeChanged(PropPlacementMode newMode);
    }

    public interface IPropSelectedObserver
    {
        void OnPropSelected(PropObject propInstance);
    }

    public interface IPropUnselectedObserver
    {
        void OnPropUnselected(PropObject propInstance);
    }
    
    public interface IPropPlacementObserver
    {
        void OnPropPlaced(Entities.Prop prop, PropObject propInstance, Tile onTile);
    }

    public interface IPropRemoveObserver
    {
        void OnPropRemoved(Entities.Prop prop, PropObject propInstance, bool retrieveIntoInventory = true);
    }

    public interface IPropPlacementValidObserver
    {
        void OnPropPlacementValid();
    }
    
    public interface IPropPlacementInvalidObserver
    {
        void OnPropPlacementInvalid();
    }
}