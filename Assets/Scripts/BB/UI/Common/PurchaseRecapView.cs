using System;
using System.Collections.Generic;
using BB.Services.Modules.Cart;
using BB.UI.Common.Components;
using TheForge.Extensions;
using TheForge.Services.Views;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.Common
{
    public class PurchaseRecapView : View
    {
        [SerializeField] private PurchaseRecapEntryComponent recapEntryComponentPrefab;
        [SerializeField] private Transform recapEntryContainer;
        [Space]
        [SerializeField] private Button inventoryButton;
        
        public Action OnInventoryClick { get; set; }
        
        private readonly List<PurchaseRecapEntryComponent> _recapEntryComponents = new();
        
        public void Initialize(List<CartEntry> cartEntries)
        {
            _recapEntryComponents.ForEach(entryComponent => Destroy(entryComponent.gameObject));
            _recapEntryComponents.Clear();
            
            foreach (var cartEntry in cartEntries)
            {
                var spawnedRecapComp = Instantiate(recapEntryComponentPrefab, recapEntryContainer);
                spawnedRecapComp.Initialize(new PurchaseRecapComponentDto
                {
                    EntitySprite = cartEntry.PurchasableEntity.Sprite,
                    EntryQuantity =  cartEntry.Quantity,
                });
                _recapEntryComponents.Add(spawnedRecapComp);
            }
            
            inventoryButton.onClick.ReplaceListeners(() => OnInventoryClick?.Invoke());
        }
    }
}