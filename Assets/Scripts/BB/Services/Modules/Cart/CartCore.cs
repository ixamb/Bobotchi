using System;
using System.Collections.Generic;
using System.Linq;
using BB.Data;
using BB.Entities;
using JetBrains.Annotations;

namespace BB.Services.Modules.Cart
{
    internal sealed class CartCore
    {
        private readonly Dictionary<string, Cart> _carts = new();

        public CartCore()
        {
            var cartTypes = Enum.GetValues(typeof(PurchasableEntityType)).Cast<PurchasableEntityType>();
            foreach (var cartType in cartTypes)
            {
                _carts.Add(cartType.ToString(), new Cart(cartType));
            }
        }
        
        public IEnumerable<CartEntry> GetCart(PurchasableEntityType cartType)
            => Cart(cartType)?.GetEntries();
        
        public void AddToCart(PurchasableEntityType cartType, PurchasableEntity entity, uint quantity)
            => Cart(cartType)?.Add(entity, quantity);
        
        public void RemoveFromCart(PurchasableEntityType cartType, PurchasableEntity entity, uint quantity)
            => Cart(cartType)?.Remove(entity, quantity);
        
        public void ClearCart(PurchasableEntityType cartType) 
            => Cart(cartType)?.Clear();
        
        public uint GetCartEntryCount(PurchasableEntityType cartType, PurchasableEntity entity)
            => Cart(cartType)?.EntryCount(entity) ?? 0;
        
        public float GetCartTotalPrice(PurchasableEntityType cartType)
            => Cart(cartType)?.TotalPrice() ?? 0;

        public uint GetTotalEntitiesInCart(PurchasableEntityType cartType)
            => Cart(cartType)?.EntryCount() ?? 0;
        
        public float GetCartTotalPricePerEntity(PurchasableEntityType cartType, PurchasableEntity entity)
            => Cart(cartType)?.TotalPricePerEntry(entity) ?? 0;
        
        [CanBeNull] private Cart Cart(PurchasableEntityType cartType) => _carts.GetValueOrDefault(cartType.ToString());
    }
    
    internal sealed class Cart
    {
        private PurchasableEntityType _type;
        private readonly Dictionary<Guid, CartEntry> _entries = new();
        
        public Cart(PurchasableEntityType cartType)
        {
            _type = cartType;
        }
        
        public IEnumerable<CartEntry> GetEntries() => _entries.Values;

        public void Add(PurchasableEntity entity, uint quantity)
        {
            if (_entries.TryGetValue(entity.Guid, out var value))
                value.IncrementQuantity(quantity);
            else
                _entries.Add(entity.Guid, new CartEntry(entity, quantity));
        }

        public void Remove(PurchasableEntity entity, uint quantity)
        {
            if (!_entries.TryGetValue(entity.Guid, out var value))
                return;
        
            value.DecrementQuantity(quantity);
            if (value.Quantity == 0)
                _entries.Remove(entity.Guid);
        }
        
        public void Clear()
        {
            _entries.Clear();
        }

        public uint EntryCount()
        {
            return (uint) _entries.Count;
        }
        
        public uint EntryCount(PurchasableEntity entity)
        {
            return _entries.TryGetValue(entity.Guid, out var value) ? value.Quantity : 0;
        }
        
        public float TotalPrice()
        {
            return _entries.Sum(entry => entry.Value.GetTotalPrice());
        }

        public float TotalPricePerEntry(PurchasableEntity entity)
        {
            return _entries[entity.Guid].GetTotalPrice();
        }
    }

    public sealed class CartEntry
    {
        public PurchasableEntity PurchasableEntity { get; private set; }
        public uint Quantity { get; private set; }

        public CartEntry(PurchasableEntity purchasableEntity, uint quantity)
        {
            PurchasableEntity = purchasableEntity;
            Quantity = quantity;
        }

        internal void IncrementQuantity(uint quantity)
        {
            Quantity += quantity;
        }
            
        internal void DecrementQuantity(uint quantity)
        {
            if (quantity > Quantity)
                Quantity = 0;
            else
                Quantity -= quantity;
        }

        public float GetTotalPrice()
        {
            return Quantity * PurchasableEntity.Price;
        }
    }
}