using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using BB.Grid.Tiles;
using Core.Services;
using UnityEngine;
using Vector2Int = Core.Types.Vector2Int;

namespace BB.Grid
{
    public sealed class GridManager : Singleton<GridManager, IGridManager>, IGridManager
    {
        [SerializeField] private GridLayerConfiguration layerConfiguration;
        
        [SerializeField] private Tile tilePrefab;
        [SerializeField] private Transform gridParent;

        private Tile[,] _tiles;

        protected override void Init()
        {
            using var gridGenerator = new GridGenerator();
            _tiles = gridGenerator.GenerateInitialGrid(
                tileStates: layerConfiguration.LayerCsvAsset(),
                tile: tilePrefab,
                gridParent: gridParent);
            
            using var wallGenerator = new WallGenerator();
            wallGenerator.GenerateWalls(_tiles);
        }

        [CanBeNull]
        public Tile GetTile(Vector2Int tileCoordinates)
        {
            var rows = _tiles.GetLength(0);
            var cols = _tiles.GetLength(1);
            
            if (tileCoordinates.X < 0 || tileCoordinates.X >= rows || tileCoordinates.Y < 0 || tileCoordinates.Y >= cols)
                return null;
            
            return _tiles[tileCoordinates.X, tileCoordinates.Y];
        }

        public IEnumerable<Tile> GetTiles()
        {
            return _tiles.Cast<Tile>();
        }

        public IEnumerable<Tile> GetTilesWithinRadius(Tile tile, uint radius, bool includeSource = true)
        {
            var tiles = new List<Tile>();
            
            var rows = _tiles.GetLength(0);
            var cols = _tiles.GetLength(1);

            (int x, int y) start =
                (Mathf.Max(0, tile.GridPosition.X - (int) radius), Mathf.Max(0, tile.GridPosition.Y - (int) radius));
            
            (int x, int y) end =
                (Mathf.Min(rows, tile.GridPosition.X + (int) radius + 1), Mathf.Min(cols, tile.GridPosition.Y + (int) radius + 1));

            for (var x = start.x; x < end.x; x++)
            {
                for (var y = start.y; y < end.y; y++)
                {
                    if (tile.GridPosition == new Vector2Int(x, y) && !includeSource)
                        continue;
                    tiles.Add(_tiles[x, y]);
                }
            }

            return tiles;
        }

        [CanBeNull]
        public Tile PickRandomFreeTileCloseToTile(Tile tile, uint fieldSize)
        {
            var closeFreeTiles = new List<Tile>();
            
            var rows = _tiles.GetLength(0);
            var cols = _tiles.GetLength(1);

            (int x, int y) start =
                (Mathf.Max(0, tile.GridPosition.X - (int) fieldSize), Mathf.Max(0, tile.GridPosition.Y - (int) fieldSize));
            
            (int x, int y) end =
                (Mathf.Min(rows, tile.GridPosition.X + (int) fieldSize + 1), Mathf.Min(cols, tile.GridPosition.Y + (int) fieldSize + 1));
            
            for (var x = start.x; x < end.x; x++)
            {
                for (var y = start.y; y < end.y; y++)
                {
                    if (_tiles[x, y].State != TileState.Free
                        || (x == tile.GridPosition.X && y == tile.GridPosition.Y))
                        continue;
                    
                    closeFreeTiles.Add(_tiles[x, y]);
                }
            }
            if (!closeFreeTiles.Any())
                return null;
            
            var index = Random.Range(0, closeFreeTiles.Count);
            return closeFreeTiles[index];
        }
        
        [CanBeNull]
        public Tile PickRandomFreeTile()
        {
            var freeTiles = new List<Tile>();
            
            var rows = _tiles.GetLength(0);
            var cols = _tiles.GetLength(1);
            
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    if (_tiles[i, j].State == TileState.Free)
                        freeTiles.Add(_tiles[i, j]);
                }
            }

            if (!freeTiles.Any())
                return null;
            
            var index = Random.Range(0, freeTiles.Count);
            return freeTiles[index];
        }
        
        public Vector2Int GetGridSize()
        {
            return new Vector2Int(_tiles.GetLength(0), _tiles.GetLength(1));
        }
    }
}