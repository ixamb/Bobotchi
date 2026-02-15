using System;
using System.Globalization;
using Core.Runtime.Extensions;
using Core.Runtime.Services.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.Common.Components
{
    public class GridEntryComponent : ViewComponent<GridEntryDto>
    {
        [SerializeField] protected Image entryImage;
        [SerializeField] protected TMP_Text entryTitle;
        [SerializeField] protected TMP_Text entryDescription;
        [SerializeField] protected TMP_Text entryQuantity;
        [SerializeField] protected Button clickableButton;

        public override void Initialize(GridEntryDto gridEntryDto)
        {
            if (gridEntryDto.Sprite is not null)
                entryImage.sprite = gridEntryDto.Sprite;
            entryTitle.text = gridEntryDto.Title;
            entryDescription.text = gridEntryDto.Description;
            entryQuantity.text = gridEntryDto.Quantity.ToString(CultureInfo.InvariantCulture);
            clickableButton.onClick.ReplaceListeners(() => gridEntryDto.OnClick?.Invoke());
        }
    }
    
    public class GridEntryDto : ComponentDto
    {
        public Sprite Sprite { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Quantity { get; set; }
        public Action OnClick { get; set; }
    }
}