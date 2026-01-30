using BB.Data;
using UnityEngine;

namespace BB.Entities
{
    public abstract class PurchasableEntity : Entity
    {
        [SerializeField] private bool availableInShop = true;
        [SerializeField] private bool singlePurchase;
        [SerializeField] private Currency currency;
        [SerializeField] private float price;
        
        public bool AvailableInShop => availableInShop;
        public bool SinglePurchase => singlePurchase;
        public Currency Currency => currency;
        public float Price => price;
    }
}