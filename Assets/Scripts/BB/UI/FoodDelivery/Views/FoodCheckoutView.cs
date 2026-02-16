using System;
using System.Collections.Generic;
using BB.Data;
using BB.Services.Modules.Cart;
using BB.UI.Common.Components;
using TheForge.Extensions;
using TheForge.Services.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.FoodDelivery.Views
{
    public sealed class FoodCheckoutView : View
    {
        [SerializeField] private CheckoutEntryComponent checkoutEntryComponent;
        [SerializeField] private Transform content;
        [Space]
        [SerializeField] private Button checkoutButton;
        [SerializeField] private TMP_Text checkoutTextButton;

        private readonly List<CheckoutEntryComponent> _components = new();
        
        public event Action OnCartConfirmed;
        
        public void Initialize()
        {
            _components.ForEach(component => Destroy(component.gameObject));
            _components.Clear();

            foreach (var cartEntry in CartService.Instance.GetCart(PurchasableEntityType.Food))
            {
                var spawnedCartEntry = Instantiate(checkoutEntryComponent, content);
                spawnedCartEntry.Initialize(cartEntry);
                _components.Add(spawnedCartEntry);
            }
            
            checkoutButton.onClick.ReplaceListeners(() => OnCartConfirmed?.Invoke());
            checkoutTextButton.text = $"Payer ({CartService.Instance.GetCartTotalPrice(PurchasableEntityType.Food)}€)";
        }
    }
}