using System;
using System.Collections.Generic;
using System.Linq;
using BB.Data;
using BB.Entities;
using Core.Runtime.Services;
using JetBrains.Annotations;

namespace BB.Services.Modules.Cart
{
    internal sealed class CartService : Singleton<CartService, ICartService>, ICartService
    {
        private readonly CartCore _cartCore = new();

        protected override void Init()
        { }

        public IEnumerable<CartEntry> GetCart(PurchasableEntityType cartType)
        {
            return _cartCore.GetCart(cartType);
        }

        public void AddToCart(PurchasableEntityType cartType, PurchasableEntity entity, uint quantity)
        {
            _cartCore.AddToCart(cartType, entity, quantity);
        }

        public void RemoveFromCart(PurchasableEntityType cartType, PurchasableEntity entity, uint quantity)
        {
            _cartCore.RemoveFromCart(cartType, entity, quantity);
        }

        public void ClearCart(PurchasableEntityType cartType)
        {
            _cartCore.ClearCart(cartType);
        }

        public uint GetCartEntryCount(PurchasableEntityType cartType, PurchasableEntity entity)
        {
            return _cartCore.GetCartEntryCount(cartType, entity);
        }

        public float GetCartTotalPrice(PurchasableEntityType cartType)
        {
            return _cartCore.GetCartTotalPrice(cartType);
        }

        public uint GetTotalEntitiesInCart(PurchasableEntityType cartType)
        {
            return _cartCore.GetTotalEntitiesInCart(cartType);
        }

        public float GetCartTotalPricePerEntity(PurchasableEntityType cartType, PurchasableEntity entity)
        {
            return _cartCore.GetCartTotalPricePerEntity(cartType, entity);
        }
    }
}