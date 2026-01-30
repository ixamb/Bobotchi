using System;
using System.Collections.Generic;
using System.Linq;
using BB.Entities;
using BB.UI.Common.Components;
using BB.UI.FoodDelivery.Components;
using Core.Services.Views;
using TMPro;
using UnityEngine;

namespace BB.UI.FoodDelivery.Views
{
    public sealed class MerchantDetailView : View
    {
        [SerializeField] private FoodEntryComponent foodEntryComponentPrefab;
        [SerializeField] private Transform foodContent;
        [SerializeField] private TMP_Text merchantTitle;

        private readonly List<FoodEntryComponent> _spawnedFoods = new();
        
        public event Action<Food> OnFoodSelected;
        
        public void Initialize(Merchant merchant)
        {
            if (_spawnedFoods.Any())
            {
                foreach (var spawned in _spawnedFoods)
                    Destroy(spawned.gameObject);
                _spawnedFoods.Clear();
            }
            
            merchantTitle.text = merchant.Name;
            foreach (var food in merchant.Foods.Where(food => food.AvailableInShop))
            {
                var spawnedFood = Instantiate(foodEntryComponentPrefab, foodContent);
                spawnedFood.Initialize(new GridEntryDto
                {
                    Sprite = food.Sprite,
                    Title = food.Name,
                    Description = food.Description,
                    Quantity = food.Price,
                    OnClick = () => OnFoodSelected?.Invoke(food)
                });
                _spawnedFoods.Add(spawnedFood);
            }
        }
    }
}