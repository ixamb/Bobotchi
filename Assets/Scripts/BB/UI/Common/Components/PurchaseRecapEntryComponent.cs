using TheForge.Services.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.Common.Components
{
    public class PurchaseRecapEntryComponent : ViewComponent<PurchaseRecapComponentDto>
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text counter;
        
        public override void Initialize(PurchaseRecapComponentDto componentDto)
        {
            if (componentDto.EntitySprite is not null)
                image.sprite = componentDto.EntitySprite;
            counter.text = $"x{componentDto.EntryQuantity}";
        }
    }

    public class PurchaseRecapComponentDto : ComponentDto
    {
        public Sprite EntitySprite { get; set; }
        public float EntryQuantity { get; set; }
    }
}