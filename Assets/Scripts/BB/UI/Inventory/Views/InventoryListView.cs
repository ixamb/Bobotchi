using System;
using System.Collections.Generic;
using System.Linq;
using BB.Data;
using BB.Entities;
using BB.Services.Modules.GameData;
using BB.Services.Modules.LocalSave;
using BB.UI.Common.Components;
using BB.UI.Inventory.Components;
using Core.Services.Views;
using JetBrains.Annotations;
using UnityEngine;

namespace BB.UI.Inventory.Views
{
    public class InventoryListView : View
    {
        [SerializeField] private InventoryEntryComponent inventoryEntryPrefab;
        [SerializeField] private Transform content;
        [SerializeField] private InventoryViewMode defaultViewMode;
        [SerializeField] private InventoryTabButtonComponent propButton;
        [SerializeField] private InventoryTabButtonComponent surfaceButton;
        [SerializeField] private InventoryTabButtonComponent foodButton;
        [Space]
        [SerializeField] private GameObject emptyComponent;
        
        private InventoryViewMode _currentViewMode;
        private readonly List<InventoryEntryComponent> _inventoryEntries = new();
        
        public Action<Prop> OnPropSelected;
        public Action<Surface> OnSurfaceSelected;
        public Action<Food> OnFoodSelected;

        public void Initialize(InventoryViewMode? forcedInventoryViewMode = null)
        {
            _currentViewMode = forcedInventoryViewMode ?? defaultViewMode;
            _inventoryEntries.ForEach(entry => Destroy(entry.gameObject));
            _inventoryEntries.Clear();
            
            switch (_currentViewMode) 
            {
                case InventoryViewMode.Prop: SwitchToPropView(); break;
                case InventoryViewMode.Surface: SwitchToSurfaceView(); break;
                case InventoryViewMode.Food: SwitchToFoodView(); break;
                default: break;
            }
        }
        
        [UsedImplicitly]
        public void SwitchToPropView()
        {
            ClearInventoryEntries();
            DisplayPropInventoryScreen();
            propButton.OnSelected();
            surfaceButton.OnUnselected();
            foodButton.OnUnselected();
        }

        public void SwitchToSurfaceView()
        {
            ClearInventoryEntries();
            DisplaySurfaceInventoryScreen();
            propButton.OnUnselected();
            surfaceButton.OnSelected();
            foodButton.OnUnselected();
        }

        public void SwitchToFoodView()
        {
            ClearInventoryEntries();
            DisplayFoodInventoryScreen();
            foodButton.OnSelected();
            surfaceButton.OnUnselected();
            propButton.OnUnselected();
        }
        
        private void DisplayPropInventoryScreen()
        {
            var propsData = GameDataService.Instance.GetProps().ToList();
            foreach (var furnitureEntry in BBLocalSaveService.Instance.PurchasableEntities.Get()
                         .Where(entry => entry.PurchasableEntityType == PurchasableEntityType.Furniture))
            {
                var propData = propsData.FirstOrDefault(furniture => furniture.Guid == furnitureEntry.EntityGuid);
                if (propData is null)
                    continue;
                
                var spawnedEntry = Instantiate(inventoryEntryPrefab, content);
                spawnedEntry.Initialize(
                    new GridEntryDto
                    {
                        Sprite = propData.Sprite,
                        Title = propData.Name,
                        Description = propData.Description,
                        Quantity = furnitureEntry.Quantity,
                        OnClick = () => OnPropSelected?.Invoke(propData),
                    });
                _inventoryEntries.Add(spawnedEntry);
            }
            emptyComponent.SetActive(!_inventoryEntries.Any());
        }

        private void DisplaySurfaceInventoryScreen()
        {
            var surfacesData = GameDataService.Instance.GetSurfaces().ToList();
            foreach (var furnitureEntry in BBLocalSaveService.Instance.PurchasableEntities.Get()
                         .Where(entry => entry.PurchasableEntityType == PurchasableEntityType.Furniture))
            {
                var surfaceData = surfacesData.FirstOrDefault(surface => surface.Guid == furnitureEntry.EntityGuid);
                if (surfaceData is null)
                    continue;
                
                var spawnedEntry = Instantiate(inventoryEntryPrefab, content);
                spawnedEntry.Initialize
                (
                    new GridEntryDto
                    {
                        Sprite = surfaceData.Sprite,
                        Title = surfaceData.Name,
                        Description = surfaceData.Description,
                        Quantity = furnitureEntry.Quantity,
                        OnClick = () => OnSurfaceSelected?.Invoke(surfaceData),
                    });
                _inventoryEntries.Add(spawnedEntry);
            }
            emptyComponent.SetActive(!_inventoryEntries.Any());
        }

        private void DisplayFoodInventoryScreen()
        {
            var foodsData = GameDataService.Instance.GetFoods().ToList();
            foreach (var furnitureEntry in BBLocalSaveService.Instance.PurchasableEntities.Get().Where(entry => entry.PurchasableEntityType == PurchasableEntityType.Food))
            {
                var spawnedEntry = Instantiate(inventoryEntryPrefab, content);
                var foodData = foodsData.FirstOrDefault(furniture => furniture.Guid == furnitureEntry.EntityGuid);
                if (foodData is null)
                    continue;
                
                spawnedEntry.Initialize(
                    new GridEntryDto
                    {
                        Sprite = foodData.Sprite,
                        Title = foodData.Name,
                        Description = foodData.Description,
                        Quantity = furnitureEntry.Quantity,
                        OnClick = () => OnFoodSelected?.Invoke(foodData),
                    });
                _inventoryEntries.Add(spawnedEntry);
            }
            emptyComponent.SetActive(!_inventoryEntries.Any());
        }
        
        private void ClearInventoryEntries()
        {
            _inventoryEntries.ForEach(entry => Destroy(entry.gameObject));
            _inventoryEntries.Clear();
        }

        public enum InventoryViewMode
        {
            Prop,
            Surface,
            Food,
        }
    }
}