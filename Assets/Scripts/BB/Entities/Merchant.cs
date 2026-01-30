using System.Collections.Generic;
using UnityEngine;

namespace BB.Entities
{
    [CreateAssetMenu(fileName = "Merchant", menuName = "BB/Entities/Merchant")]
    public sealed class Merchant : Entity
    {
        [SerializeField] private List<Food> foods = new List<Food>();
        
        public List<Food> Foods => foods;
    }
}