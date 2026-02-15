using System;
using System.Collections.Generic;
using BB.Data;
using BB.Entities;
using BB.Management.FurniturePlacement;
using BB.Services.Modules.LocalSave;
using BB.UI.Inventory.Views;
using BB.UI.PropPlacement;
using Core.Runtime.Services.Views;

namespace BB.UI.Inventory
{
    public class InventoryViewCoordinator
    {
        private readonly InventoryListView _inventoryListView;
        private readonly InventoryDetailView _inventoryDetailView;

        private readonly Stack<View> _views = new();

        private readonly Action _onExit;
        
        public InventoryViewCoordinator(InventoryListView inventoryListView, InventoryDetailView inventoryDetailView, Action onExit)
        {
            _inventoryListView = inventoryListView;
            _inventoryDetailView = inventoryDetailView;
            
            _views.Push(_inventoryListView);
            HookEvents();
            _onExit = onExit;
        }

        private void HookEvents()
        {
            _inventoryListView.OnPropSelected += furniture =>
            {
                _inventoryDetailView.Initialize(PurchasableEntityType.Furniture, furniture);
                ShowDetailView();
            };

            _inventoryListView.OnSurfaceSelected += furniture =>
            {
                _inventoryDetailView.ShowView();
                ShowDetailView();
            };

            _inventoryListView.OnFoodSelected += food =>
            {
                _inventoryDetailView.Initialize(PurchasableEntityType.Food, food);
                ShowDetailView();
            };

            _inventoryDetailView.OnInventoryActionTriggered += (type, entity) =>
            {
                NavigateBack();
                switch (type)
                {
                    case PurchasableEntityType.Furniture:
                    {
                        if (entity is Prop prop)
                        {
                            NavigateBack();
                            NavigateBack();
                            var furniturePlacementView =
                                ViewService.Instance.GetView<PropPlacementModeView>("furniture-placement-view");
                            furniturePlacementView.ShowView();
                            furniturePlacementView.ActivateEditModeWithPropSelected(prop);
                            break;
                        }

                        if (entity is Surface {SurfaceType: SurfaceType.Floor} floorSurface)
                        {
                            NavigateBack();
                            BBLocalSaveService.Instance.PurchasableEntities.Update(PurchasableEntityType.Furniture, floorSurface, UpdateOperation.Add, 1);
                            FurniturePlacementManager.Instance.PlaceFloor(floorSurface);
                            BBLocalSaveService.Instance.FurniturePlacement.Place(floorSurface);
                            break;
                        }

                        if (entity is Surface {SurfaceType: SurfaceType.Wall} wallSurface)
                        {
                            NavigateBack();
                            BBLocalSaveService.Instance.PurchasableEntities.Update(PurchasableEntityType.Furniture, wallSurface, UpdateOperation.Add, 1);
                            FurniturePlacementManager.Instance.PlaceWall(wallSurface);
                            BBLocalSaveService.Instance.FurniturePlacement.Place(wallSurface);
                            break;
                        }

                        break;
                    }
                    case PurchasableEntityType.Food:
                    {
                        NavigateBack();
                        _inventoryListView.Initialize(InventoryListView.InventoryViewMode.Food);
                        (entity as Food)?.EffectActions.ForEach(effect => effect.Execute());
                        BBLocalSaveService.Instance.PurchasableEntities.Update(PurchasableEntityType.Food, entity, UpdateOperation.Remove, 1);
                        BBLocalSaveService.Instance.Save();
                        break;
                    }
                }
            };
        }

        private void ShowDetailView()
        {
            _inventoryDetailView.ShowView();
            _views.Push(_inventoryDetailView);
        }

        public void NavigateBack()
        {
            if (_views.Count <= 1)
            {
                _onExit?.Invoke();
                return;
            }
            
            _views.Pop().HideView();
        }
    }
}