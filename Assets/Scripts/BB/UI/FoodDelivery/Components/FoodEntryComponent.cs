using BB.UI.Common.Components;

namespace BB.UI.FoodDelivery.Components
{
    public sealed class FoodEntryComponent : GridEntryComponent
    {
        public override void Initialize(GridEntryDto gridEntryDto)
        {
            base.Initialize(gridEntryDto);
            entryQuantity.text = $"{gridEntryDto.Quantity}€";
        }
    }
}