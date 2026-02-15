using System;
using System.Collections.Generic;
using System.Linq;
using BB.Data;
using BB.Entities;
using BB.Services.Modules.Cart;
using Core.Runtime.Utils;
using NUnit.Framework;
using Tests.TestUtils;
using UnityEngine;

namespace Tests.ServiceTests
{
    public class CartTests
    {
        [TestCase(PurchasableEntityType.Food)]
        [TestCase(PurchasableEntityType.Furniture)]
        public void AddSinglePurchasableEntityToCart_ShouldSuccess(PurchasableEntityType purchasableEntityType)
        {
            var entity = CreatePurchasableEntity(purchasableEntityType);
            var cart = new CartCore();
    
            cart.AddToCart(purchasableEntityType, entity, 1);
    
            var cartItems = cart.GetCart(purchasableEntityType);
            var cartItem = cartItems.FirstOrDefault(item => item.PurchasableEntity.Equals(entity));
    
            Assert.IsNotNull(cartItem, "Entity should be in cart");
            Assert.AreEqual(1, cartItem.Quantity, "Quantity should be 1");
        }

        [TestCase(PurchasableEntityType.Food, 3u)]
        [TestCase(PurchasableEntityType.Furniture, 10u)]
        public void AddMultiplePurchasableEntitiesToCart_ShouldSuccess(PurchasableEntityType purchasableEntityType, uint quantity)
        {
            var entity = CreatePurchasableEntity(purchasableEntityType);
            var cart = new CartCore();
    
            cart.AddToCart(purchasableEntityType, entity, quantity);
            Assert.AreEqual(quantity, cart.GetCartEntryCount(purchasableEntityType, entity), $"Quantity should be {quantity}");
        }

        [TestCase(PurchasableEntityType.Food, 3u)]
        [TestCase(PurchasableEntityType.Furniture, 10u)]
        public void RemoveSinglePurchasableEntityFromCart_ShouldSuccess(PurchasableEntityType purchasableEntityType, uint baseQuantity)
        {
            var entity = CreatePurchasableEntity(purchasableEntityType);
            var cart = new CartCore();
            
            cart.AddToCart(purchasableEntityType, entity, (uint)baseQuantity);
            Assume.That(cart.GetCartEntryCount(purchasableEntityType, entity), Is.EqualTo(baseQuantity));
            
            cart.RemoveFromCart(purchasableEntityType, entity, 1);
            Assert.AreEqual(baseQuantity-1, cart.GetCartEntryCount(purchasableEntityType, entity), $"Quantity should be {baseQuantity-1}");
        }

        [TestCase(PurchasableEntityType.Food, 3u, 2u)]
        [TestCase(PurchasableEntityType.Furniture, 10u, 5u)]
        public void RemoveMultiplePurchasableEntitiesFromCart_ShouldSuccess(PurchasableEntityType purchasableEntityType, uint baseQuantity, uint quantityToRemove)
        {
            var entity = CreatePurchasableEntity(purchasableEntityType);
            var cart = new CartCore();
            
            cart.AddToCart(purchasableEntityType, entity, baseQuantity);
            Assume.That(cart.GetCartEntryCount(purchasableEntityType, entity), Is.EqualTo(baseQuantity));
            
            cart.RemoveFromCart(purchasableEntityType, entity ,quantityToRemove);
            Assert.AreEqual(baseQuantity-quantityToRemove, cart.GetCartEntryCount(purchasableEntityType, entity), $"Quantity should be {baseQuantity-quantityToRemove}");
        }

        [TestCase(PurchasableEntityType.Food, 3u, 7u)]
        [TestCase(PurchasableEntityType.Furniture, 10u, 15u)]
        public void RemoveMorePurchasableEntitiesThanPresent_ShouldSuccess(PurchasableEntityType purchasableEntityType, uint baseQuantity, uint quantityToRemove)
        {
            var entity = CreatePurchasableEntity(purchasableEntityType);
            var cart = new CartCore();
            
            cart.AddToCart(purchasableEntityType, entity, baseQuantity);
            Assume.That(cart.GetCartEntryCount(purchasableEntityType, entity), Is.EqualTo(baseQuantity));
            
            cart.RemoveFromCart(purchasableEntityType, entity, quantityToRemove);
            Assert.AreEqual(0, cart.GetCartEntryCount(purchasableEntityType, entity), $"Quantity should be {0}");
        }

        [TestCase(PurchasableEntityType.Food)]
        [TestCase(PurchasableEntityType.Furniture)]
        public void ClearCart_ShouldSuccess(PurchasableEntityType purchasableEntityType)
        {
            var entity = CreatePurchasableEntity(purchasableEntityType);
            var cart = new CartCore();
            
            cart.AddToCart(purchasableEntityType, entity, 1);
            Assume.That(cart.GetCartEntryCount(purchasableEntityType, entity), Is.EqualTo(1));
            
            cart.ClearCart(purchasableEntityType);
            Assert.AreEqual(0, cart.GetCartEntryCount(purchasableEntityType, entity), $"Quantity should be {0}");
        }
        
        [TestCase(PurchasableEntityType.Food, 3u)]
        [TestCase(PurchasableEntityType.Furniture, 10u)]
        public void VerifyCartTotalPrice_ShouldSuccess(PurchasableEntityType purchasableEntityType, uint quantity)
        {
            var entity = CreatePurchasableEntity(purchasableEntityType);
            var cart = new CartCore();
            
            cart.AddToCart(purchasableEntityType, entity, quantity);
            Assume.That(cart.GetCartEntryCount(purchasableEntityType, entity), Is.EqualTo(quantity));
            Assert.AreEqual(entity.Price*quantity, cart.GetCartTotalPrice(purchasableEntityType), $"Price should be {entity.Price*quantity}");
        }

        [TestCaseSource(nameof(GeneratePurchasableEntitiesTestCase))]
        public void VerifyCartTotalPrice_ShouldSuccess(IEnumerable<CartItemData> cartItemData)
        {
            var cart = new CartCore();
            Dictionary<PurchasableEntityType, float> totalCartPriceDictionary = new();
            foreach (var item in cartItemData)
            {
                cart.AddToCart(item.Type, item.Entity, item.Quantity);
                
                if (totalCartPriceDictionary.ContainsKey(item.Type))
                    totalCartPriceDictionary[item.Type] += item.Quantity * item.Entity.Price;
                else
                    totalCartPriceDictionary.Add(item.Type, item.Quantity * item.Entity.Price);
            }
            
            if (totalCartPriceDictionary.TryGetValue(PurchasableEntityType.Food, out var foodCartPrice))
                Assert.AreEqual(foodCartPrice, cart.GetCartTotalPrice(PurchasableEntityType.Food), $"Price should be {foodCartPrice}");
            
            if (totalCartPriceDictionary.TryGetValue(PurchasableEntityType.Furniture, out var furnitureCartPrice))
                Assert.AreEqual(furnitureCartPrice, cart.GetCartTotalPrice(PurchasableEntityType.Furniture), $"Price should be {furnitureCartPrice}");
        }

        [TestCaseSource(nameof(GeneratePurchasableEntitiesTestCase))]
        public void VerifyCartTotalEntities_ShouldSuccess(IEnumerable<CartItemData> cartItemData)
        {
            var cart = new CartCore();
            foreach (var item in cartItemData)
            {
                cart.AddToCart(item.Type, item.Entity, item.Quantity);
            }
            
            Assert.AreEqual(cartItemData.Where(item => item.Type == PurchasableEntityType.Food).Count(), cart.GetTotalEntitiesInCart(PurchasableEntityType.Food));
            Assert.AreEqual(cartItemData.Where(item => item.Type == PurchasableEntityType.Furniture).Count(), cart.GetTotalEntitiesInCart(PurchasableEntityType.Furniture));
        }

        [TestCase(PurchasableEntityType.Food, 3u)]
        [TestCase(PurchasableEntityType.Furniture, 10u)]
        public void VerifyCartTotalPricePerEntry_ShouldSuccess(PurchasableEntityType purchasableEntityType, uint quantity)
        {
            var entity = CreatePurchasableEntity(purchasableEntityType);
            var cart = new CartCore();
            
            cart.AddToCart(purchasableEntityType, entity, quantity);
            Assume.That(cart.GetCartEntryCount(purchasableEntityType, entity), Is.EqualTo(quantity));
            Assert.AreEqual(entity.Price*quantity, cart.GetCartTotalPricePerEntity(purchasableEntityType, entity), $"Price should be {entity.Price*quantity}");
        }

        #region purchasable entity generators

        private static IEnumerable<TestCaseData> GeneratePurchasableEntitiesTestCase()
        {
            yield return new TestCaseData(new List<CartItemData>
            {
                new() { Type = PurchasableEntityType.Food, Quantity = 11u, Entity = GenerateRandomPurchasableEntity(PurchasableEntityType.Food)},
                new() { Type = PurchasableEntityType.Food, Quantity = 15u, Entity = GenerateRandomPurchasableEntity(PurchasableEntityType.Food)},
                new() { Type = PurchasableEntityType.Food, Quantity = 30u, Entity = GenerateRandomPurchasableEntity(PurchasableEntityType.Food)},
            }).SetName("Food_Set");
    
            yield return new TestCaseData(new List<CartItemData>
            {
                new() { Type = PurchasableEntityType.Furniture, Quantity = 2, Entity = GenerateRandomPurchasableEntity(PurchasableEntityType.Furniture)},
                new() { Type = PurchasableEntityType.Furniture, Quantity = 5, Entity = GenerateRandomPurchasableEntity(PurchasableEntityType.Furniture)},
                new() { Type = PurchasableEntityType.Furniture, Quantity = 6, Entity = GenerateRandomPurchasableEntity(PurchasableEntityType.Furniture)},
            }).SetName("Furniture_Set");
            
            yield return new TestCaseData(new List<CartItemData>
            {
                new() { Type = PurchasableEntityType.Food, Quantity = 10, Entity = GenerateRandomPurchasableEntity(PurchasableEntityType.Food)},
                new() { Type = PurchasableEntityType.Food, Quantity = 15, Entity = GenerateRandomPurchasableEntity(PurchasableEntityType.Food)},
                new() { Type = PurchasableEntityType.Furniture, Quantity = 3, Entity = GenerateRandomPurchasableEntity(PurchasableEntityType.Furniture)},
            }).SetName("Mixed_Set");
        }
        
        private static PurchasableEntity GenerateRandomPurchasableEntity(PurchasableEntityType purchasableEntityType)
        {
            return CreatePurchasableEntity(purchasableEntityType, StringUtils.Random(8), StringUtils.Random(20), UnityEngine.Random.Range(0, 150));
        }
        
        private static PurchasableEntity CreatePurchasableEntity(PurchasableEntityType purchasableEntityType, string name, string description, float price)
        {
            var purchasableEntity = CreatePurchasableEntity(purchasableEntityType);
            TestScriptableObjectHelper.SetPrivateField(purchasableEntity, "name", name);
            TestScriptableObjectHelper.SetPrivateField(purchasableEntity, "description", description);
            TestScriptableObjectHelper.SetPrivateField(purchasableEntity, "price", price);
            return purchasableEntity;
        }
        
        private static PurchasableEntity CreatePurchasableEntity(PurchasableEntityType purchasableEntityType)
        {
            PurchasableEntity purchasableEntity = purchasableEntityType switch
            {
                PurchasableEntityType.Food => ScriptableObject.CreateInstance<Food>(),
                PurchasableEntityType.Furniture => ScriptableObject.CreateInstance<Prop>(),
                _ => throw new ArgumentOutOfRangeException(nameof(purchasableEntityType), purchasableEntityType, null)
            };
            TestScriptableObjectHelper.SetPrivateField(purchasableEntity,"guid", Guid.NewGuid().ToString());
            return purchasableEntity;
        }
        
        #endregion purchasable entity generators
        
        public class CartItemData
        {
            public PurchasableEntityType Type { get; set; }
            public uint Quantity { get; set; }
            public PurchasableEntity Entity { get; set; }
        }
    }
}
