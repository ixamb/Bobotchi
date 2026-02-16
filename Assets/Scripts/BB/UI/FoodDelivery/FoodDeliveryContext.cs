using BB.Data;
using BB.UI.Common;
using BB.UI.Common.Components;
using BB.UI.FoodDelivery.Views;
using TheForge.Extensions;
using TheForge.Services.Views;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.FoodDelivery
{
    public sealed class FoodDeliveryContext : MonoBehaviour
    {
        [SerializeField] private View foodDeliveryView;
        [SerializeField] private MerchantListView merchantListView;
        [SerializeField] private MerchantDetailView merchantDetailView;
        [SerializeField] private FoodDetailView foodDetailView;
        [SerializeField] private CheckoutView foodCheckoutView;
        [SerializeField] private PurchaseRecapView purchaseRecapView;
        [SerializeField] private CartComponent cartComponent;
        [SerializeField] private Button navigateBackButton;
        
        private FoodDeliveryCoordinator _coordinator;

        private void Start()
        {
            _coordinator = new FoodDeliveryCoordinator(cartComponent, foodCheckoutView, purchaseRecapView, PurchasableEntityType.Food, Exit);
            _coordinator.Initialize(merchantListView, merchantDetailView, foodDetailView);
            
            navigateBackButton.onClick.ReplaceListeners(() => _coordinator.NavigateBack());
        }

        private void Exit()
        {
            foodDeliveryView.HideView();
        }
    }
}