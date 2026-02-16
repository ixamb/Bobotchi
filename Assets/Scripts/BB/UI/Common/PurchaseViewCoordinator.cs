using System;
using System.Collections.Generic;
using System.Linq;
using BB.Data;
using BB.Entities;
using BB.Services.Modules.Cart;
using BB.Services.Modules.LocalSave;
using BB.UI.Common.Components;
using BB.UI.Inventory.Views;
using BB.UI.Main;
using TheForge.Services.Views;

namespace BB.UI.Common
{
    public abstract class PurchaseViewCoordinator
    {
        protected readonly Stack<View> Views = new();
        protected readonly CheckoutView CheckoutView;
        protected readonly PurchaseRecapView PurchaseRecapView;
        protected readonly CartComponent CartComponent;
        private readonly PurchasableEntityType cartType;
        private event Action OnExit;
        
        protected PurchaseViewCoordinator(CartComponent cartComponent, CheckoutView checkoutView, PurchaseRecapView purchaseRecapView, PurchasableEntityType cartType, Action onExit)
        {
            CartComponent = cartComponent;
            CheckoutView = checkoutView;
            PurchaseRecapView = purchaseRecapView;
            this.cartType = cartType;
            OnExit = onExit;
        }
        
        protected void UpdateCart(PurchasableEntity entity, uint quantity)
        {
            var cartByEntity = CartService.Instance.GetCartEntryCount(cartType, entity);
            if (quantity > cartByEntity)
            {
                CartService.Instance.AddToCart(cartType, entity, quantity - cartByEntity);
            }
            else if (quantity < cartByEntity)
            {
                CartService.Instance.RemoveFromCart(cartType, entity, cartByEntity - quantity);
            }
            CartComponent.UpdateComponent();
            /*
            var cartByEntity = CartService.Instance.GetCartEntryCount(cartType, entity);
            var total = quantity - cartByEntity;
            if (total < quantity)
                CartService.Instance.RemoveFromCart(cartType, entity, total);
            else
                CartService.Instance.AddToCart(cartType, entity, total);
            CartComponent.UpdateComponent();*/
        }
        
        protected bool TryConfirmCart()
        {
            foreach (var purchasableFood in CartService.Instance.GetCart(cartType))
            {
                BBLocalSaveService.Instance.PurchasableEntities.Update(cartType, purchasableFood.PurchasableEntity, UpdateOperation.Add, purchasableFood.Quantity);
                BBLocalSaveService.Instance.Balance.Update(purchasableFood.PurchasableEntity.Currency, -CartService.Instance.GetCartTotalPricePerEntity(cartType, purchasableFood.PurchasableEntity));
            }
            BBLocalSaveService.Instance.Save();
            ViewService.Instance.GetView<StatusBarView>("status-bar-view").UpdateView();
            Views.Pop().HideView();
            
            ShowPurchaseRecapView(CartService.Instance.GetCart(cartType).ToList());
            CartService.Instance.ClearCart(cartType);
            
            return true;
        }
        
        protected void ShowCheckoutView()
        {
            CheckoutView.Initialize();
            CheckoutView.ShowView();
            Views.Push(CheckoutView);
        }

        private void ShowPurchaseRecapView(List<CartEntry> cartEntries)
        {
            PurchaseRecapView.OnInventoryClick += () =>
            {
                var view = ViewService.Instance.GetView<View>("inventory-view");
                view.GetComponentInChildren<InventoryListView>().Initialize(InventoryListView.InventoryViewMode.Prop);
                view.ShowView();
            };
            
            PurchaseRecapView.Initialize(cartEntries);
            PurchaseRecapView.ShowView();
            Views.Push(PurchaseRecapView);
        }

        public void NavigateAllBack()
        {
            while (Views.Count > 1)
                NavigateBack();
        }
        
        public void NavigateBack()
        {
            if (Views.Count <= 1)
            {
                OnExit?.Invoke();
                return;
            }
            
            Views.Pop().HideView();
        }
    }
}