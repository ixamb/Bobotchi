using System;
using BB.Grid.Tiles;
using UnityEngine;
using Object = UnityEngine.Object;
using Vector2Int = Core.Types.Vector2Int;

namespace BB.Grid
{
    public sealed class GridGenerator : IDisposable
    {
        public Tile[,] GenerateInitialGrid(TileState[,] tileStates, Tile tile, Transform gridParent)
        {
            var spatialTileSize = tile.transform.lossyScale;
            var gridSize = new Vector2Int(tileStates.GetLength(0), tileStates.GetLength(1));

            var tiles = new Tile[gridSize.X, gridSize.Y];
            
            var halfWidth = gridSize.X / 2;
            var halfHeight = gridSize.Y / 2;
            
            for (var i = 0; i < gridSize.X; i++)
            {
                for (var j = 0; j < gridSize.Y; j++)
                {
                    var tileObject = Object.Instantiate(tile, gridParent);
                    tileObject.name = $"Tile-{i},{j}";
                    tileObject.transform.localPosition = new Vector3((i-halfWidth)*spatialTileSize.x, 0, (j-halfHeight)*spatialTileSize.y);
                    tileObject.transform.parent = gridParent;
                    tileObject.Initialize(new Vector2Int(i,j), tileStates[i, j]);
                    
                    tiles[i, j] = tileObject;
                }
            }

            return tiles;
        }

        public void Dispose()
        {
        }
    }
}