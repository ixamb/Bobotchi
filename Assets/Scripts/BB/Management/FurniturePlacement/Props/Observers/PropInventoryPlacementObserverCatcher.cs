using BB.Data;
using BB.Grid.Tiles;
using BB.Services.Modules.LocalSave;
using UnityEngine;

namespace BB.Management.FurniturePlacement.Props.Observers
{
    public class PropInventoryPlacementObserverCatcher : MonoBehaviour, IPropPlacementObserver, IPropRemoveObserver
    {
        private void Start()
        {
            FurniturePlacementManager.Instance.RegisterPlacementObserver(this);
            FurniturePlacementManager.Instance.RegisterRemoveObserver(this);
        }

        public void OnPropPlaced(Entities.Prop prop, PropObject propInstance, Tile onTile)
        {
            var isMoveRequest = BBLocalSaveService.Instance.FurniturePlacement.GetPlacementSave(propInstance.PlacementGuid) is not null;
            
            BBLocalSaveService.Instance.FurniturePlacement.Place(
                prop: prop,
                instance: propInstance,
                position: new Vector2(onTile.GridPosition.X, onTile.GridPosition.Y),
                direction: propInstance.Rotation());
            
            if (!isMoveRequest)
                BBLocalSaveService.Instance.PurchasableEntities.Update(PurchasableEntityType.Furniture, prop, UpdateOperation.Remove, 1);
            BBLocalSaveService.Instance.Save();
        }

        public void OnPropRemoved(Entities.Prop prop, PropObject propInstance, bool retrieveIntoInventory = true)
        {
            if (!retrieveIntoInventory)
                return;
            
            BBLocalSaveService.Instance.FurniturePlacement.RemovePlaced(propInstance.PlacementGuid, autoSave: true);
            BBLocalSaveService.Instance.PurchasableEntities.Update(PurchasableEntityType.Furniture, prop, UpdateOperation.Add, 1);
            BBLocalSaveService.Instance.Save();
        }
    }
}