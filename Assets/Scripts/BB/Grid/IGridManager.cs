using System.Collections.Generic;
using BB.Grid.Tiles;
using Core.Runtime.Services;
using Core.Runtime.Types;

namespace BB.Grid
{
    public interface IGridManager : ISingleton
    {
        Tile GetTile(Vector2Int tileCoordinates);
        IEnumerable<Tile> GetTiles();
        IEnumerable<Tile> GetTilesWithinRadius(Tile tile, uint radius, bool includeSource = true);
        Tile PickRandomFreeTileCloseToTile(Tile tile, uint fieldSize);
        Tile PickRandomFreeTile();
        Vector2Int GetGridSize();
    }
}