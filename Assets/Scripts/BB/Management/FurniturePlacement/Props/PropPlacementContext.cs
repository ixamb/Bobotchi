using BB.Grid.Tiles;
using JetBrains.Annotations;
using UnityEngine;

namespace BB.Management.FurniturePlacement.Props
{
    public sealed class PropPlacementContext
    {
        public PropObject PropObject { get; private set; }
        public bool IsFromInventory { get; private set; }
        
        public Tile OriginTile { get; private set; }
        public PropPlacementDirection OriginDirection { get; private set; }
        
        public void UpdateFromInventory(Entities.Prop prop)
        {
            PropObject = prop.PropPrefab;
            IsFromInventory = true;
        }

        public void UpdateFromScene(GameObject propObject, [CanBeNull] Tile originTile, PropPlacementDirection originDirection)
        {
            PropObject = propObject.GetComponent<PropObject>();
            OriginTile = originTile;
            OriginDirection = originDirection;
            IsFromInventory = false;
        }
        
        public void Clear()
        {
            PropObject = null;
            IsFromInventory = false;
            OriginTile = null;
        }
    }
}