using System;
using System.Linq;
using BB.Data;
using BB.Entities;
using BB.Grid;
using BB.Services.Modules.GameData;
using BB.Services.Modules.LocalSave;

namespace BB.Management.FurniturePlacement.Surfaces
{
    public interface ISurfacePlacementService
    {
        void LoadPlacedSurfaces();
        void PlaceSurfaceOnGround(Surface surface);
        void PlaceSurfaceOnWalls(Surface surface);
    }
    
    public sealed class SurfacePlacementService : IDisposable, ISurfacePlacementService
    {
        public void LoadPlacedSurfaces()
        {
            var placedWall = BBLocalSaveService.Instance.FurniturePlacement.GetSurfacePlacementGuid(SurfaceType.Wall);
            var placedGround = BBLocalSaveService.Instance.FurniturePlacement.GetSurfacePlacementGuid(SurfaceType.Floor);
            
            PlaceSurfaceOnWalls(GameDataService.Instance.GetWall(placedWall));
            PlaceSurfaceOnGround(GameDataService.Instance.GetFloor(placedGround));
        }
        
        public void PlaceSurfaceOnGround(Surface surface)
        {
            if (surface.SurfaceType != SurfaceType.Floor)
                return;

            var tiles = GridManager.Instance.GetTiles();
            
            foreach (var tile in tiles)
            {
                tile.UpdateRenderer(surface.Sprite);
            }
        }
        
        public void PlaceSurfaceOnWalls(Surface surface)
        {
            if (surface.SurfaceType != SurfaceType.Wall)
                return;

            var walls = GridManager.Instance.GetTiles().SelectMany(wall => wall.Walls);
            
            foreach (var wall in walls)
            {
                wall.UpdateRenderer(surface.Sprite);
            }
        }

        public void Dispose()
        {
        }
    }
}