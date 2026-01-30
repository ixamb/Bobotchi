using System;
using System.Collections.Generic;
using BB.Entities;
using BB.UI.FoodDelivery.Components;
using Core.Services.Views;
using UnityEngine;

namespace BB.UI.FoodDelivery.Views
{
    public sealed class MerchantListView : View
    {
        [Space]
        [SerializeField] private MerchantComponent merchantPrefab;
        [SerializeField] private Transform merchantContent;
        
        public event Action<Merchant> OnMerchantSelected;

        public void Initialize(IEnumerable<Merchant> merchants)
        {
            foreach (var merchant in merchants)
            {
                var spawnedMerchant = Instantiate(merchantPrefab, merchantContent);
                spawnedMerchant.Initialize(new MerchantComponentDto()
                {
                    MerchantSprite = merchant.Sprite,
                    MerchantTitle = merchant.Name,
                    MerchantDescription = merchant.Description,
                    OnClick = () => OnMerchantSelected?.Invoke(merchant)
                });
            }
        }
    }
}