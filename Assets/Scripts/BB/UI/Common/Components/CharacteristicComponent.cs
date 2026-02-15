using Core.Runtime.Services.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.Common.Components
{
    public sealed class CharacteristicComponent : ViewComponent<CharacteristicComponentDto>
    {
        [SerializeField] private Image characteristicImage;
        [SerializeField] private TMP_Text characteristicText;

        public override void Initialize(CharacteristicComponentDto componentDto)
        {
            if (componentDto.Sprite is not null)
                characteristicImage.sprite = componentDto.Sprite;
            characteristicText.text = componentDto.Description;
        }
    }

    public class CharacteristicComponentDto : ComponentDto
    {
        public Sprite Sprite { get; set; }
        public string Description { get; set; }
    }
}