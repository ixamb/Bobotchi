using BB.Data;
using Core.Services.Views;
using TMPro;
using UnityEngine;

namespace BB.UI.FurnitureDelivery.Components
{
    public sealed class FurnitureCollectionTitleComponent : ViewComponent<FurnitureCollectionTitleDto>
    {
        [SerializeField] private TMP_Text text;

        private FurnitureCollection _furnitureCollection;
        
        public override void Initialize(FurnitureCollectionTitleDto furnitureCollection)
        {
            _furnitureCollection = furnitureCollection.Collection;
            text.text = furnitureCollection.Collection.ToTranslatedString();
        }
        
        public FurnitureCollection GetFurnitureCollection() => _furnitureCollection;
    }

    public class FurnitureCollectionTitleDto : ComponentDto
    {
        public FurnitureCollection Collection { get; set; }
    }
}