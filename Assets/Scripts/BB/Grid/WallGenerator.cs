using System;
using BB.Grid.Tiles;
using Core.Types;

namespace BB.Grid
{
    public class WallGenerator : IDisposable
    {
        public void GenerateWalls(Tile[,] tiles)
        {
            var gridSize = new Vector2Int(tiles.GetLength(0), tiles.GetLength(1));

            for (var i = 0; i < gridSize.X; i++)
            {
                for (var j = 0; j < gridSize.Y; j++)
                {
                    if (i == 0)
                        tiles[i, j].SetLeftWallActive(true);
                    if (j == gridSize.Y - 1)
                        tiles[i, j].SetRightWallActive(true);
                }
            }
        }

        public void Dispose()
        {
        }
    }
}