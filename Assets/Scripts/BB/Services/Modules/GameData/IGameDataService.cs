using System;
using System.Collections.Generic;
using BB.Data;
using BB.Entities;
using Core.Services;
using UnityEngine;

namespace BB.Services.Modules.GameData
{
    public interface IGameDataService : ISingleton
    {
        IGameDataOptions GameOptions();
        
        Sprite GetSprite(string key);
        IEnumerable<Furniture> GetFurnitures();
        Prop GetProp(Guid propGuid);
        IEnumerable<Prop> GetProps();
        IEnumerable<Surface> GetSurfaces();
        Surface GetFloor(Guid floorGuid);
        IEnumerable<Surface> GetFloors();
        Surface GetWall(Guid wallGuid);
        IEnumerable<Surface> GetWalls();
        IEnumerable<Merchant> GetMerchants();
        IEnumerable<Food> GetFoods();
    }
}