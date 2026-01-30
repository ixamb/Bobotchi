using System;
using System.Collections.Generic;
using BB.Data;
using UnityEngine;

namespace BB.Entities
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "BB/Entities/Recipe")]
    public sealed class Recipe : PurchasableEntity
    {
        [SerializeField] private RecipeCategory category;
        [SerializeField] private List<IngredientEntry> ingredients;
        [SerializeField] private Food result;
        
        public RecipeCategory Category => category;
        public List<IngredientEntry> Ingredients => ingredients;
        public Food Result => result;
    }

    [Serializable]
    public sealed class IngredientEntry
    {
        [SerializeField] private Food ingredient;
        [SerializeField] private int quantity;
    }
}