using System;
using System.Collections.Generic;
using BB.Data;
using BB.Entities;
using BB.Services.Modules.GameData;
using BB.UI.Common.Components;
using TheForge.Extensions;
using TheForge.Services.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.Inventory.Views
{
    public class InventoryDetailView : View
    {
        [SerializeField] private CharacteristicComponent characteristicPrefab;
        [SerializeField] private Transform content;
        [Space]
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        [Space]
        [SerializeField] private Button actionButton;
        [SerializeField] private TMP_Text actionButtonText;

        public Action<PurchasableEntityType, PurchasableEntity> OnInventoryActionTriggered { get; set; }
        
        private readonly List<CharacteristicComponent> _characteristicComponents = new();
        
        public void Initialize(PurchasableEntityType type, PurchasableEntity entity)
        {
            _characteristicComponents.ForEach(entry => Destroy(entry.gameObject));
            _characteristicComponents.Clear();
            
            if (entity.Sprite is not null)
                image.sprite = entity.Sprite;
            title.text = entity.Name;
            description.text = entity.ExtendedDescription;
            
            actionButton.onClick.ReplaceListeners(() => OnInventoryActionTriggered?.Invoke(type, entity));

            switch (type)
            {
                case PurchasableEntityType.Food:
                {
                    actionButtonText.text = "Manger (miam)";
                    InitializeCharacteristicComponents(entity as Food);
                    break;
                }
                case PurchasableEntityType.Furniture:
                {
                    actionButtonText.text = "Placer dans l'appart'";
                    if (entity is Prop prop)
                        InitializeCharacteristicComponents(prop);
                    if (entity is Surface floor)
                        InitializeCharacteristicComponents(floor);
                    break;
                }
            }
        }
        
        private void InitializeCharacteristicComponents(Prop prop)
        {
            var collectionCharacteristic = Instantiate(characteristicPrefab, content);
            collectionCharacteristic.Initialize(new CharacteristicComponentDto
            {
                Sprite = GameDataService.Instance.GetSprite("furniture-collection-icon"),
                Description = prop.Collection.ToTranslatedString(),
            });
            _characteristicComponents.Add(collectionCharacteristic);
            
            var categoryCharacteristic = Instantiate(characteristicPrefab, content);
            categoryCharacteristic.Initialize(new CharacteristicComponentDto
            {
                Sprite = GameDataService.Instance.GetSprite("furniture-category-icon"),
                Description = prop.Category.ToTranslatedString(),
            });
            _characteristicComponents.Add(categoryCharacteristic);
            
            var sizeCharacteristic = Instantiate(characteristicPrefab, content);
            sizeCharacteristic.Initialize(new CharacteristicComponentDto
            {
                Sprite = GameDataService.Instance.GetSprite("furniture-size-icon"),
                Description = $"{prop.Size.X}x{prop.Size.Y}",
            });
            _characteristicComponents.Add(sizeCharacteristic);
        }

        private void InitializeCharacteristicComponents(Surface surface)
        {
            var collectionCharacteristic = Instantiate(characteristicPrefab, content);
            collectionCharacteristic.Initialize(new CharacteristicComponentDto
            {
                Sprite = GameDataService.Instance.GetSprite("furniture-collection-icon"),
                Description = surface.Collection.ToTranslatedString(),
            });
            _characteristicComponents.Add(collectionCharacteristic);
            
            var surfaceTypeCharacteristic = Instantiate(characteristicPrefab, content);
            surfaceTypeCharacteristic.Initialize(new CharacteristicComponentDto
            {
                Sprite = GameDataService.Instance.GetSprite("furniture-category-icon"),
                Description = surface.SurfaceType.ToTranslatedString(),
            });
            _characteristicComponents.Add(surfaceTypeCharacteristic);
        }

        private void InitializeCharacteristicComponents(Food food)
        {
            food.EffectActions.ForEach(effect =>
            {
                var characteristicEffect = Instantiate(characteristicPrefab, content);
                characteristicEffect.Initialize(new CharacteristicComponentDto
                {
                    Sprite = GameDataService.Instance.GetSprite(StatIconEntryKey(effect.AffectedState)),
                    Description =
                        $"{(effect.Affection >= 0 ? "+" : string.Empty)}{effect.Affection} : {effect.AffectedState.ToTranslatedString()}",
                });
                _characteristicComponents.Add(characteristicEffect);
            });
            
            return;

            string StatIconEntryKey(CharacterStateStat state)
            {
                return state switch
                {
                    CharacterStateStat.Energy => "state-energy-icon",
                    CharacterStateStat.Hunger => "state-hunger-icon",
                    CharacterStateStat.Esteem => "state-esteem-icon",
                    _ => string.Empty
                };
            }
        }
    }
}