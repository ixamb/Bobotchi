using System;
using TheForge.Services.Views;
using TheForge.UI.Components.SingleSelectable;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.Settings.Components
{
    [RequireComponent(typeof(SingleSelectableElement))]
    public class BackgroundColorPickerComponent : ViewComponent<BackgroundColorPickerDto>
    {
        [SerializeField] private Image formerColor;
        [SerializeField] private GameObject checkObject;
        [SerializeField] private Button clickable;
        
        public override void Initialize(BackgroundColorPickerDto componentInitParameters)
        {
            formerColor.color = componentInitParameters.Color;
            clickable.onClick.AddListener(() =>
            {
                componentInitParameters.OnClick();
            });
            GetComponent<SingleSelectableElement>().OnSelected(componentInitParameters.IsActive);
        }
    }

    public sealed class BackgroundColorPickerDto : ComponentDto
    {
        public Color Color { get; set; }
        public bool IsActive { get; set; }
        public Action OnClick { get; set; }
    }
}