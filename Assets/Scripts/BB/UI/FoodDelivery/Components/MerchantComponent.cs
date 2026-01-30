using System;
using Core.Extensions;
using Core.Services.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.FoodDelivery.Components
{
    public sealed class MerchantComponent : ViewComponent<MerchantComponentDto>
    {
        [SerializeField] private Image merchantImage;
        [SerializeField] private TMP_Text merchantTitle;
        [SerializeField] private TMP_Text merchantDescription;
        [SerializeField] private Button clickableButton;

        public override void Initialize(MerchantComponentDto componentInitParameters)
        {
            if (componentInitParameters.MerchantSprite is not null)
                merchantImage.sprite = componentInitParameters.MerchantSprite;
            merchantTitle.text = componentInitParameters.MerchantTitle;
            merchantDescription.text = componentInitParameters.MerchantDescription;
            clickableButton.onClick.ReplaceListeners(() => componentInitParameters.OnClick?.Invoke());
        }
    }

    public class MerchantComponentDto : ComponentDto
    {
        public Sprite MerchantSprite { get; set; }
        public string MerchantTitle { get; set; }
        public string MerchantDescription { get; set; }
        public Action OnClick { get; set; }
    }
}