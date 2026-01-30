using BB.Data;
using BB.Entities;
using BB.Services.Modules.GameData;
using BB.UI.Common;
using BB.UI.Common.Components;

namespace BB.UI.FurnitureDelivery.Views
{
    public sealed class FurniturePurchaseDetailView : PurchaseDetailView
    {
        public override void Initialize(PurchasableEntity purchasableEntity)
        {
            if (purchasableEntity is not Furniture)
                return;
            base.Initialize(purchasableEntity);
        }

        protected override void InitializePurchasableCharacteristics(PurchasableEntity purchasableEntity)
        {
            if (purchasableEntity is not Furniture furniture)
                return;
            
            if (furniture is Prop prop)
                InitializePropCharacteristics(prop);
            
            if (furniture is Surface floor)
                InitializeFloorCharacteristics(floor);
        }

        private void InitializePropCharacteristics(Prop prop)
        {
            var collectionCharacteristic = Instantiate(characteristicPrefab, characteristicsParent);
            collectionCharacteristic.Initialize(new CharacteristicComponentDto
            {
                Sprite = GameDataService.Instance.GetSprite("furniture-collection-icon"),
                Description = prop.Collection.ToTranslatedString(),
            });
            CharacteristicComponents.Add(collectionCharacteristic);
            
            var categoryCharacteristic = Instantiate(characteristicPrefab, characteristicsParent);
            categoryCharacteristic.Initialize(new CharacteristicComponentDto
            {
                Sprite = GameDataService.Instance.GetSprite("furniture-category-icon"),
                Description = prop.Category.ToTranslatedString(),
            });
            CharacteristicComponents.Add(categoryCharacteristic);
            
            var sizeCharacteristic = Instantiate(characteristicPrefab, characteristicsParent);
            sizeCharacteristic.Initialize(new CharacteristicComponentDto()
            {
                Sprite = GameDataService.Instance.GetSprite("furniture-size-icon"),
                Description = $"{prop.Size.X}x{prop.Size.Y}",
            });
            CharacteristicComponents.Add(sizeCharacteristic);
        }

        private void InitializeFloorCharacteristics(Surface surface)
        {
            var collectionCharacteristic = Instantiate(characteristicPrefab, characteristicsParent);
            collectionCharacteristic.Initialize(new CharacteristicComponentDto
            {
                Sprite = GameDataService.Instance.GetSprite("furniture-collection-icon"),
                Description = surface.Collection.ToTranslatedString(),
            });
            CharacteristicComponents.Add(collectionCharacteristic);
            
            var surfaceTypeCharacteristic = Instantiate(characteristicPrefab, characteristicsParent);
            surfaceTypeCharacteristic.Initialize(new CharacteristicComponentDto
            {
                Sprite = GameDataService.Instance.GetSprite("furniture-category-icon"),
                Description = surface.SurfaceType.ToTranslatedString(),
            });
            CharacteristicComponents.Add(surfaceTypeCharacteristic);
        }
    }
}