using System;
using System.Collections.Generic;
using System.Linq;
using BB.Data;
using BB.Entities;
using BB.Management.FurniturePlacement;
using BB.Management.FurniturePlacement.Props;
using UnityEngine;

namespace BB.Services.Modules.LocalSave.Handlers
{
    public sealed class FurniturePlacementSaveHandler : SaveHandler<FurniturePlacementSaveDto>
    {
        protected override string ConstantEntryKey => Constants.LocalSaveEntries.PropPlacements;
        
        public void Place(Prop prop, PropObject instance, Vector2 position, PropPlacementDirection direction, bool autoSave = false)
        {
            var furniturePlacementSaveDto = GetData();
            var existingPlacementEntry = furniturePlacementSaveDto.PropPlaced.FirstOrDefault(entry => entry.PlacementEntryGuid == instance.Guid);
            if (existingPlacementEntry is not null)
            {
                existingPlacementEntry.GridX = (int) position.x;
                existingPlacementEntry.GridY = (int) position.y;
                existingPlacementEntry.Direction = direction;
            }
            else
            {
                furniturePlacementSaveDto.PropPlaced.Add(
                    new FurniturePlacementSaveDto.PropPlacementEntrySaveDto
                    {
                        PlacementEntryGuid = instance.Guid,
                        PropGuid = prop.Guid,
                        GridX = (int) position.x,
                        GridY = (int) position.y,
                        Direction = direction
                    });
            }
            SetData(furniturePlacementSaveDto, autoSave);
        }

        public void RemovePlaced(Guid placementGuid, bool autoSave = false)
        {
            var furniturePlacementSaveDto = GetData();
            var removable = furniturePlacementSaveDto.PropPlaced.FirstOrDefault(placement => placement.PlacementEntryGuid == placementGuid);
            if (removable is null)
                return;
            
            furniturePlacementSaveDto.PropPlaced.Remove(removable);
            SetData(furniturePlacementSaveDto, autoSave);
        }

        public void Place(Surface surface, bool autoSave = false)
        {
            var furniturePlacementSaveDto = GetData();
            switch (surface.SurfaceType)
            {
                case SurfaceType.Floor: furniturePlacementSaveDto.FloorGuid = surface.Guid; break;
                case SurfaceType.Wall: furniturePlacementSaveDto.WallGuid = surface.Guid; break;
                default: return;
            }
            SetData(furniturePlacementSaveDto, autoSave);
        }
        
        public Guid GetSurfacePlacementGuid(SurfaceType surfaceType)
        {
            var furniturePlacementSaveDto = GetData();
            return surfaceType switch
            {
                SurfaceType.Floor => furniturePlacementSaveDto.FloorGuid,
                SurfaceType.Wall => furniturePlacementSaveDto.WallGuid,
                _ => throw new ArgumentOutOfRangeException(nameof(surfaceType), surfaceType, null)
            };
        }

        public IEnumerable<FurniturePlacementSaveDto.PropPlacementEntrySaveDto> GetAllPlaced()
        {
            return GetData().PropPlaced;
        }

        public FurniturePlacementSaveDto.PropPlacementEntrySaveDto GetPlacementSave(Guid placementGuid)
        {
            return GetData().PropPlaced.FirstOrDefault(placement => placement.PlacementEntryGuid == placementGuid);
        }

        public FurniturePlacementSaveDto.PropPlacementEntrySaveDto GetPlacementSave(Vector2 position)
        {
            return GetData().PropPlaced.FirstOrDefault(placement => placement.GridX == (int)position.x && placement.GridY == (int)position.y);
        }
    }
}