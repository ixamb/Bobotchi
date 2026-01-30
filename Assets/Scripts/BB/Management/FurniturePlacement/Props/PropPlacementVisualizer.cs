using BB.Grid.Tiles;
using UnityEngine;

namespace BB.Management.FurniturePlacement.Props
{
    public interface IPropPlacementVisualizer
    {
        PropObject SpawnProp(Entities.Prop prop);
        void PlaceOnTile(PropObject propObject, Tile tile, PropPlacementDirection direction);
        void DestroyProp(PropObject propObject);
    }
    
    public sealed class PropPlacementVisualizer : IPropPlacementVisualizer
    {
        public PropObject SpawnProp(Entities.Prop prop)
        {
            return Object.Instantiate(prop.PropPrefab);
        }

        public void PlaceOnTile(PropObject propObject, Tile tile, PropPlacementDirection direction)
        {
            propObject.transform.SetParent(tile.GetPropAnchor);
            propObject.transform.localPosition = Vector3.zero;
            propObject.transform.localRotation = Quaternion.identity;
            propObject.Rotate(direction);
        }

        public void DestroyProp(PropObject propObject)
        {
            Object.Destroy(propObject.gameObject);
        }
    }
}