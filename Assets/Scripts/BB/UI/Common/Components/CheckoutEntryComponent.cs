using BB.Services.Modules.Cart;
using TheForge.Services.Views;
using TMPro;
using UnityEngine;

namespace BB.UI.Common.Components
{
    public sealed class CheckoutEntryComponent : ViewComponent<CheckoutEntryComponentDto>
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private TMP_Text totalPrice;
        
        public void Initialize(CartEntry cartEntry)
        {
            text.text = $"x{cartEntry.Quantity} : {cartEntry.PurchasableEntity.Name}";
            totalPrice.text = $"{cartEntry.Quantity * cartEntry.PurchasableEntity.Price}€";
        }

        public override void Initialize(CheckoutEntryComponentDto checkoutComponentDto)
        {
            text.text = $"x{checkoutComponentDto.EntityQuantity} : {checkoutComponentDto.EntityName}";
            totalPrice.text = $"{checkoutComponentDto.EntityQuantity * checkoutComponentDto.EntityPrice}€";
        }
    }

    public class CheckoutEntryComponentDto : ComponentDto
    {
        public string EntityName { get; set; }
        public int EntityQuantity { get; set; }
        public float EntityPrice { get; set; }
    }
}