using System;
using BB.Data;
using BB.Services.Modules.Cart;
using TheForge.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.Common.Components
{
    public sealed class CartComponent : MonoBehaviour
    {
        [SerializeField] private PurchasableEntityType cartType;
        [SerializeField] private TMP_Text cartText;
        [SerializeField] private Button interactableButton;
        
        public event Action OnClick;
        
        private void Start()
        {
            UpdateComponent();
            interactableButton.onClick.ReplaceListeners(() => OnClick?.Invoke());
        }

        public void UpdateComponent()
        {
            interactableButton.interactable = CartService.Instance.GetTotalEntitiesInCart(cartType) > 0;
            cartText.text = $"{CartService.Instance.GetCartTotalPrice(cartType)}€";
        }
    }
}