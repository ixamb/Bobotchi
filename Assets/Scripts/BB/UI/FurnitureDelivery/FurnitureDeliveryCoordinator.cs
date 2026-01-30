using System;
using System.Linq;
using BB.Data;
using BB.Entities;
using BB.Services.Modules.GameData;
using BB.Services.Modules.LocalSave;
using BB.UI.Common;
using BB.UI.Common.Components;
using BB.UI.FurnitureDelivery.Views;

namespace BB.UI.FurnitureDelivery
{
    public sealed class FurnitureDeliveryCoordinator : PurchaseViewCoordinator
    {
        private FurnitureListView _furnitureListView;
        private FurniturePurchaseDetailView furniturePurchaseDetailView;
        
        public FurnitureDeliveryCoordinator(CartComponent cartComponent, CheckoutView checkoutView, PurchaseRecapView purchaseRecapView, PurchasableEntityType cartType, Action onExit)
            : base(cartComponent, checkoutView, purchaseRecapView, cartType, onExit)
        { }

        public void Initialize(FurnitureListView furnitureListView, FurniturePurchaseDetailView furniturePurchaseDetailView)
        {
            _furnitureListView = furnitureListView;
            this.furniturePurchaseDetailView = furniturePurchaseDetailView;
            HookEvents();
            InitializeFurnitureListView();
        }

        private void HookEvents()
        {
            _furnitureListView.OnFurnitureSelected += ShowFurnitureDetailView;
            furniturePurchaseDetailView.Cart.OnAddToCartClick += (furniture, u) =>
            {
                UpdateCart(furniture, u);
                NavigateBack();
            };
            CheckoutView.OnCartConfirmed += () =>
            {
                TryConfirmCart();
                CartComponent.UpdateComponent();
            };
            
            CartComponent.OnClick += () =>
            {
                if (!CheckoutView.IsVisibleAndActive())
                    ShowCheckoutView();
                else
                    NavigateBack();
            };
        }
        
        private void InitializeFurnitureListView()
        {
            /*
            var furnituresByCollection = GameDataService.Instance
                .GetFurnitures()
                .Where(furniture => !furniture.SinglePurchase || BBLocalSaveService.Instance.PurchasableEntities.Get(furniture.Guid) is null)
                .GroupBy(furniture => furniture.Collection) 
                .ToDictionary(group => group.Key, group => group.ToList());
            
            _furnitureListView.Initialize(furnituresByCollection);*/
            Views.Push(_furnitureListView);
        }

        private void ShowFurnitureDetailView(Furniture furniture)
        {
            furniturePurchaseDetailView.Initialize(furniture);
            furniturePurchaseDetailView.ShowView();
            Views.Push(furniturePurchaseDetailView);
        }
    }
}