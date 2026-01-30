using BB.Grid;
using UnityEngine;

namespace BB.Management.FurniturePlacement.Props.Observers
{
    public class PropSelectionColliderHandlerObserverCatcher : MonoBehaviour, IPropSelectedObserver, IPropUnselectedObserver
    {
        private void Start()
        {
            FurniturePlacementManager.Instance.RegisterPropSelectedObserver(this);
            FurniturePlacementManager.Instance.RegisterPropUnselectedObserver(this);
        }

        public void OnPropSelected(PropObject propInstance)
        {
            foreach (var tile in GridManager.Instance.GetTiles())
            {
                if (tile.GetPropAnchor.childCount == 0)
                    continue;
                
                foreach (var prop in tile.GetComponentsInChildren<PropObject>())
                {
                    if (prop == propInstance)
                        continue;
                    prop.DisableColliderInteraction();
                }
            }
        }

        public void OnPropUnselected(PropObject propInstance)
        {
            foreach (var tile in GridManager.Instance.GetTiles())
            {
                if (tile.GetPropAnchor.childCount == 0)
                    continue;
                
                foreach (var prop in tile.GetComponentsInChildren<PropObject>())
                {
                    if (prop == propInstance)
                        continue;
                    prop.EnableColliderInteraction();
                }
            }
        }
    }
}