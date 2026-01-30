using System;
using BB.Data;
using BB.Entities;
using BB.Services.Modules.Cart;
using BB.Services.Modules.GameData;
using BB.UI.Common;
using BB.UI.Common.Components;
using BB.UI.FoodDelivery.Views;

namespace BB.UI.FoodDelivery
{
    public sealed class FoodDeliveryCoordinator : PurchaseViewCoordinator
    {
        private MerchantListView _merchantListView;
        private MerchantDetailView _merchantDetailView;
        private FoodDetailView _foodDetailView;

        public FoodDeliveryCoordinator(CartComponent cartComponent, CheckoutView checkoutView, PurchaseRecapView purchaseRecapView, PurchasableEntityType cartType, Action onExit)
            : base(cartComponent, checkoutView, purchaseRecapView, cartType, onExit) { }

        public void Initialize(
            MerchantListView merchantListView,
            MerchantDetailView merchantDetailView,
            FoodDetailView foodDetailView)
        {
            _merchantListView = merchantListView;
            _merchantDetailView = merchantDetailView;
            _foodDetailView = foodDetailView;
            HookEvents();
            InitializeMerchantListView();
        }

        private void HookEvents()
        {
            _merchantListView.OnMerchantSelected += ShowMerchantDetailView;
            _merchantDetailView.OnFoodSelected += ShowFoodDetailView;
            _foodDetailView.Cart.OnAddToCartClick += (entity, u) =>
            {
                UpdateCart(entity, u);
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

        private void InitializeMerchantListView()
        {
            _merchantListView.Initialize(GameDataService.Instance.GetMerchants());
            Views.Push(_merchantListView);
        }

        private void ShowMerchantDetailView(Merchant merchant)
        {
            _merchantDetailView.Initialize(merchant);
            _merchantDetailView.ShowView();
            Views.Push(_merchantDetailView);
        }

        private void ShowFoodDetailView(Food food)
        {
            _foodDetailView.Initialize(food);
            _foodDetailView.ShowView();
            Views.Push(_foodDetailView);
        }
    }
}