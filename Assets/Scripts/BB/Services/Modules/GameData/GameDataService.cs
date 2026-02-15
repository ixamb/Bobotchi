using System;
using System.Collections.Generic;
using System.Linq;
using BB.Data;
using BB.Entities;
using Core.Runtime.Services;
using JetBrains.Annotations;
using UnityEngine;

namespace BB.Services.Modules.GameData
{
    internal sealed class GameDataService : Singleton<GameDataService, IGameDataService>, IGameDataService
    {
        [SerializeField] private GameDataOptions gameOptions;
        [SerializeField] private GameDataContainer dataContainer;
        
        protected override void Init()
        {
        }
        
        #region game options
        
        public IGameDataOptions GameOptions() => gameOptions;
        
        #endregion game options

        #region visual getter functions

        [CanBeNull]
        public Sprite GetSprite(string key)
            => dataContainer.GetSprite(key);
        
        #endregion visual getter functions
        
        #region furniture getter functions

        public IEnumerable<Furniture> GetFurnitures()
            => dataContainer.Furnitures;
        
        [CanBeNull]
        public Prop GetProp(Guid propGuid)
            => dataContainer.Furnitures.FirstOrDefault(f => f.Guid == propGuid) as Prop;
        
        public IEnumerable<Prop> GetProps()
            => dataContainer.Furnitures.OfType<Prop>();
        
        [CanBeNull]
        public Surface GetFloor(Guid floorGuid)
            => GetSurfaces().FirstOrDefault(f => f.Guid == floorGuid);
        
        public IEnumerable<Surface> GetFloors()
            => GetSurfaces().Where(surface => surface.SurfaceType == SurfaceType.Floor);
        
        [CanBeNull]
        public Surface GetWall(Guid wallGuid)
            => GetSurfaces().FirstOrDefault(f => f.Guid == wallGuid);
        
        public IEnumerable<Surface> GetWalls()
            => GetSurfaces().Where(surface => surface.SurfaceType == SurfaceType.Wall);
        
        public IEnumerable<Surface> GetSurfaces()
            => dataContainer.Furnitures.OfType<Surface>();
        
        #endregion furniture getter functions
        
        #region merchant-food getter functions

        public IEnumerable<Merchant> GetMerchants()
            => dataContainer.Merchants;
        
        public IEnumerable<Food> GetFoods()
            => dataContainer.Merchants.SelectMany(entry => entry.Foods);
        
        #endregion merchant-food getter functions
    }
}