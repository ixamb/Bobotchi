using System.Linq;
using BB.Data;
using BB.Services.Modules.GameData;
using BB.Services.Modules.LocalSave;
using BB.UI.Common;
using BB.UI.Common.Components;
using BB.UI.FurnitureDelivery.Views;
using TheForge.Extensions;
using TheForge.Services.Views;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.FurnitureDelivery
{
    public sealed class FurnitureDeliveryContext : MonoBehaviour
    {
        [SerializeField] private View furnitureDeliveryView;
        [SerializeField] private FurnitureListView furnitureListView;
        [SerializeField] private FurniturePurchaseDetailView furniturePurchaseDetailView;
        [SerializeField] private CheckoutView checkoutView;
        [SerializeField] private PurchaseRecapView purchaseRecapView;
        [SerializeField] private CartComponent cartComponent;
        [SerializeField] private Button navigateBackButton;
        
        private FurnitureDeliveryCoordinator _coordinator;

        private void Start()
        {
            _coordinator = new FurnitureDeliveryCoordinator(cartComponent, checkoutView, purchaseRecapView, PurchasableEntityType.Furniture, Exit);
            _coordinator.Initialize(furnitureListView, furniturePurchaseDetailView);
            navigateBackButton.onClick.ReplaceListeners(() => _coordinator.NavigateBack());

            furnitureDeliveryView.OnShow += () =>
            {
                var furnituresByCollection = GameDataService.Instance
                    .GetFurnitures()
                    .Where(furniture =>
                        !furniture.SinglePurchase ||
                        BBLocalSaveService.Instance.PurchasableEntities.Get(furniture.Guid) is null)
                    .GroupBy(furniture => furniture.Collection)
                    .ToDictionary(group => group.Key, group => group.ToList());

                furnitureListView.Initialize(furnituresByCollection);
            };
        }

        private void Exit()
        {
            furnitureDeliveryView.HideView();
        }
    }
}