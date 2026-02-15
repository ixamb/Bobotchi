using System.Collections.Generic;
using System.Linq;
using BB.Data;
using BB.Grid;
using BB.Grid.Tiles;
using Core.Runtime.Types;

namespace BB.Management.FurniturePlacement.Props.Utils
{
    public static class PropPlacementUtils
    {
        public static IEnumerable<Tile> GetTilesOccupiedByProp(Entities.Prop prop, PropObject propObject)
        {
            var tiles = new List<Tile>();

            if (!propObject.TryGetParentTile(out var parentTile))
            {
                return tiles;
            }
            
            if (propObject.Rotation() == PropPlacementDirection.Left)
            {
                for (var i = 0; i < prop.Size.X; i++)
                {
                    for (var j = 0; j < prop.Size.Y; j++)
                    {
                        tiles.Add(GridManager.Instance.GetTile(parentTile.GridPosition + new Vector2Int(-i, j)));
                    }
                }
            }
            else if (propObject.Rotation() == PropPlacementDirection.Right)
            {
                for (var i = 0; i < prop.Size.Y; i++)
                {
                    for (var j = 0; j < prop.Size.X; j++)
                    {
                        tiles.Add(GridManager.Instance.GetTile(parentTile.GridPosition + new Vector2Int(-i, j)));
                    }
                }
            }

            return tiles;
        }
        
        public static IEnumerable<PropObject> GetPropsWithinRadius(Tile baseTile, uint radius, PropCategory propCategory)
        {
            var propsWithinRadius = new List<PropObject>();
            
            var tilesWithinRadius = GridManager.Instance.GetTilesWithinRadius(baseTile, radius, includeSource: false).ToList();
            if (!tilesWithinRadius.Any())
                return propsWithinRadius;
            
            tilesWithinRadius.ForEach(withinRadiusTile
                => propsWithinRadius.AddRange(withinRadiusTile.GetPropAnchor.GetComponentsInChildren<PropObject>()
                    .Where(propObject => propObject.PropCategory == propCategory)));
            
            return propsWithinRadius;
        }
    }
}