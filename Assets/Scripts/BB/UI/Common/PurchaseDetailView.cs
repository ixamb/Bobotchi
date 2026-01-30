using System.Collections.Generic;
using BB.Data;
using BB.Entities;
using BB.Services.Modules.Cart;
using BB.Services.Modules.LocalSave;
using BB.UI.Common.Components;
using Core.Services.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.Common
{
    public abstract class PurchaseDetailView : View
    {
        [SerializeField] protected CharacteristicComponent characteristicPrefab;
        [SerializeField] protected Transform characteristicsParent;
        [Space]
        [SerializeField] protected Image image;
        [SerializeField] protected TMP_Text title;
        [SerializeField] protected TMP_Text description;
        [Space]
        [SerializeField] protected CartControlComponent cart;

        protected readonly List<CharacteristicComponent> CharacteristicComponents = new();
        
        public virtual void Initialize(PurchasableEntity purchasableEntity)
        {
            if (purchasableEntity.Sprite is not null)
                image.sprite = purchasableEntity.Sprite;
            
            title.text = purchasableEntity.Name;
            description.text = purchasableEntity.ExtendedDescription;
            
            var entityType = purchasableEntity is Furniture ? PurchasableEntityType.Furniture : PurchasableEntityType.Food;

            var quantity = CartService.Instance.GetCartEntryCount(entityType, purchasableEntity);
            cart.Initialize(new CartControlDto(
                entity: purchasableEntity,
                quantity: quantity,
                totalCart: CartService.Instance.GetCartTotalPrice(entityType),
                balance: BBLocalSaveService.Instance.Balance.Get(purchasableEntity.Currency)));
            
            CharacteristicComponents.ForEach(component => Destroy(component.gameObject));
            CharacteristicComponents.Clear();
            InitializePurchasableCharacteristics(purchasableEntity);
        }

        protected abstract void InitializePurchasableCharacteristics(PurchasableEntity purchasableEntity);
        
        public CartControlComponent Cart => cart;
    }
}