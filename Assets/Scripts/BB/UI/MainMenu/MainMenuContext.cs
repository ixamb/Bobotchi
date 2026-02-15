using Core.Runtime.Extensions;
using Core.Runtime.Services.Views;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.MainMenu
{
    public sealed class MainMenuContext : View
    {
        [SerializeField] private Button furnitureDeliveryButton;
        [SerializeField] private Button foodDeliveryButton;
        [SerializeField] private Button closeButton;
        
        private void Start()
        {
            BindFurnitureDeliveryButton();
            BindFoodDeliveryButton();
            BindCloseButton();
        }

        private void BindFurnitureDeliveryButton()
        {
            furnitureDeliveryButton.onClick.ReplaceListeners(() => ViewService.Instance.GetView("furniture-delivery-view").ShowView());
        }

        private void BindFoodDeliveryButton()
        {
            foodDeliveryButton.onClick.ReplaceListeners(() => ViewService.Instance.GetView("food-delivery-view").ShowView());
        }

        private void BindCloseButton()
        {
            closeButton.onClick.ReplaceListeners(HideView);
        }
    }
}