using BB.Data;
using UnityEngine;

namespace BB.Entities
{
    
    public abstract class Furniture : PurchasableEntity
    {
        [Space]
        [SerializeField] private FurnitureCollection collection;
        
        public FurnitureCollection Collection => collection;
    }
}