using System.Collections.Generic;
using BB.Data;
using BB.Entities;
using TheForge.Services;

namespace BB.Services.Modules.Cart
{
    public interface ICartService : ISingleton
    {
        IEnumerable<CartEntry> GetCart(PurchasableEntityType cartType);
        void AddToCart(PurchasableEntityType cartType, PurchasableEntity entity, uint quantity);
        void RemoveFromCart(PurchasableEntityType cartType, PurchasableEntity entity, uint quantity);
        void ClearCart(PurchasableEntityType cartType);
        uint GetCartEntryCount(PurchasableEntityType cartType, PurchasableEntity entity);
        float GetCartTotalPrice(PurchasableEntityType cartType);
        uint GetTotalEntitiesInCart(PurchasableEntityType cartType);
        float GetCartTotalPricePerEntity(PurchasableEntityType cartType, PurchasableEntity entity);
    }
}