using System;
using System.Collections.Generic;
using System.Linq;
using BB.Data;
using BB.Entities;
using JetBrains.Annotations;
using UnityEngine;

namespace BB.Services.Modules.LocalSave.Handlers
{
    public sealed class PurchasableEntitySaveHandler : SaveHandler<InventorySaveDto>
    {
        protected override string ConstantEntryKey => Constants.LocalSaveEntries.Inventory;
        
        public void Update(PurchasableEntityType type, PurchasableEntity purchasableEntity,
            UpdateOperation updateOperation, uint amount, bool autoSave = false)
        {
            var inventorySaveDto = GetData();
            var purchasableEntry = inventorySaveDto.PurchasableEntitiesInInventory.FirstOrDefault(entry => entry.EntityGuid == purchasableEntity.Guid);
            if (purchasableEntry is null)
            {
                inventorySaveDto.PurchasableEntitiesInInventory.Add(new InventorySaveDto.InventoryEntrySaveDto
                {
                    EntityGuid = purchasableEntity.Guid,
                    PurchasableEntityType = type,
                    Quantity = amount
                });
            }
            else
            {
                purchasableEntry.Quantity = updateOperation == UpdateOperation.Add
                    ? purchasableEntry.Quantity + amount
                    : (uint)Mathf.Max(purchasableEntry.Quantity - amount, 0);
                
                /*
                if (purchasableEntry.Quantity == 0)
                    inventorySaveDto.PurchasableEntitiesInInventory.Remove(purchasableEntry);*/
            }
            SetData(inventorySaveDto);
        }
        
        [CanBeNull]
        public InventorySaveDto.InventoryEntrySaveDto Get(Guid purchasableEntityGuid)
        {
            return GetData().PurchasableEntitiesInInventory.FirstOrDefault(entry => entry.EntityGuid == purchasableEntityGuid);
        }
        
        public IEnumerable<InventorySaveDto.InventoryEntrySaveDto> Get()
        {
            return GetData().PurchasableEntitiesInInventory;
        }
    }
}