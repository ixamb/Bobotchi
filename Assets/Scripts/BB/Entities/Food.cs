using System.Collections.Generic;
using BB.Actions;
using UnityEngine;

namespace BB.Entities
{
    [CreateAssetMenu(fileName = "Food", menuName = "BB/Entities/Food")]
    public sealed class Food : PurchasableEntity
    {
        [SerializeField] private List<StateStatEffectAction> effectActions;
        
        public List<StateStatEffectAction> EffectActions => effectActions;
    }
}