using System;
using System.Collections.Generic;
using BB.Data;
using BB.Services.Modules.Cart;
using BB.UI.Common.Components;
using Core.Extensions;
using Core.Services.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.Common
{
    public sealed class CheckoutView : View
    {
        [Space]
        [SerializeField] private PurchasableEntityType cartType;
        [Space]
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

            foreach (var cartEntry in CartService.Instance!.GetCart(cartType))
            {
                var spawnedCartEntry = Instantiate(checkoutEntryComponent, content);
                spawnedCartEntry.Initialize(cartEntry);
                _components.Add(spawnedCartEntry);
            }
            
            if (CartService.Instance.GetTotalEntitiesInCart(cartType) == 0)
            {
                checkoutButton.interactable = false;
                checkoutTextButton.text = $"Aucun élément dans le panier";
            }
            else
            {
                checkoutButton.onClick.ReplaceListeners(() => OnCartConfirmed?.Invoke());
                checkoutTextButton.text = $"Payer ({CartService.Instance.GetCartTotalPrice(cartType)}€)";
            }
        }
    }
}