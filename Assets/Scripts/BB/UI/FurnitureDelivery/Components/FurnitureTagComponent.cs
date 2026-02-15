using System;
using BB.Data;
using BB.Services.Modules.GameData;
using Core.Runtime.Extensions;
using Core.Runtime.Services.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.FurnitureDelivery.Components
{
    public sealed class FurnitureTagComponent : ViewComponent<FurnitureTagDto>
    {
        [SerializeField] private Image tagImage;
        [SerializeField] private TMP_Text tagText;
        [SerializeField] private Button clickableButton;

        public void Initialize(FurnitureCollection collection, Action<FurnitureCollection> onCollectionSelected)
        {
            var sprite = GetSpriteByCollection(collection);
            if (sprite is not null)
                tagImage.sprite = sprite;
            tagText.text = collection.ToTranslatedString();
            clickableButton.onClick.ReplaceListeners(() => onCollectionSelected(collection));
        }
        
        public override void Initialize(FurnitureTagDto componentInitParameters)
        {
            var sprite = GetSpriteByCollection(componentInitParameters.FurnitureCollection);
            if (sprite is not null)
                tagImage.sprite = sprite;
            tagText.text = componentInitParameters.FurnitureCollection.ToTranslatedString();
            clickableButton.onClick.ReplaceListeners(()
                => componentInitParameters.OnCollectionSelected(componentInitParameters.FurnitureCollection));
        }

        private static Sprite GetSpriteByCollection(FurnitureCollection collection)
        {
            var collectionTag = collection switch
            {
                FurnitureCollection.Basics => "furniture-basics-collection-icon",
                FurnitureCollection.Pop => "furniture-pop-collection-icon",
                FurnitureCollection.Seventies => "furniture-seventies-collection-icon",
                FurnitureCollection.Cottage => "furniture-cottage-collection-icon",
                FurnitureCollection.Minimalist => "furniture-minimalist-collection-icon",
                FurnitureCollection.London => "furniture-london-collection-icon",
                FurnitureCollection.Surf => "furniture-surf-collection-icon",
                _ => string.Empty,
            };
            return GameDataService.Instance.GetSprite(collectionTag);
        }
    }

    public class FurnitureTagDto : ComponentDto
    {
        public FurnitureCollection FurnitureCollection { get; set; }
        public Action<FurnitureCollection> OnCollectionSelected { get; set; }
    }
}