using BB.UI.Common.Components;

namespace BB.UI.FurnitureDelivery.Components
{
    public sealed class FurnitureEntryComponent : GridEntryComponent
    {
        public override void Initialize(GridEntryDto gridEntryDto)
        {
            base.Initialize(gridEntryDto);
            entryQuantity.text = $"{gridEntryDto.Quantity}€";
        }
    }
}