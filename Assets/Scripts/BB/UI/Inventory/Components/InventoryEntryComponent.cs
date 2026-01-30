using System.Globalization;
using BB.UI.Common.Components;

namespace BB.UI.Inventory.Components
{
    public class InventoryEntryComponent : GridEntryComponent
    {
        public override void Initialize(GridEntryDto gridEntryDto)
        {
            base.Initialize(gridEntryDto);
            entryQuantity.text = $"x{((int)gridEntryDto.Quantity).ToString(CultureInfo.InvariantCulture)}";
            
            if (gridEntryDto.Quantity == 0)
                clickableButton.interactable = false;
        }
    }
}