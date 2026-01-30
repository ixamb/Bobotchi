using System;
using System.Collections.Generic;
using System.Linq;
using BB.Data;
using BB.Entities;
using BB.UI.Common.Components;
using BB.UI.FurnitureDelivery.Components;
using BB.UI.Utils;
using Core.Extensions;
using Core.Services.Views;
using UnityEngine;

namespace BB.UI.FurnitureDelivery.Views
{
    public sealed class FurnitureListView : View
    {
        [Space]
        [SerializeField] private FurnitureTagComponent furnitureTagComponentPrefab;
        [SerializeField] private Transform furnitureTagContent;
        [Space]
        [SerializeField] private GridContainerComponent gridContainerComponentPrefab;
        [Space]
        [SerializeField] private FurnitureEntryComponent furnitureEntryComponentPrefab;
        [SerializeField] private FurnitureCollectionTitleComponent furnitureCollectionTitleComponentPrefab;
        [SerializeField] private Transform furnitureEntriesContent;
        [Space]
        [SerializeField] private ScrollToElement scrollToElement;

        public event Action<Furniture> OnFurnitureSelected;
        
        private readonly List<FurnitureCollectionTitleComponent> _titleComponents = new();
        private readonly List<GridContainerComponent> _gridContainerComponents = new();
        
        public void Initialize(Dictionary<FurnitureCollection, List<Furniture>> furnitures)
        {
            _titleComponents.DestroyAndClear();
            _gridContainerComponents.DestroyAndClear();
            
            foreach (var furnitureCollectionType in EnumExtensions.GetValues<FurnitureCollection>())
            {
                if (!furnitures.TryGetValue(furnitureCollectionType, out var furnituresByCollection) ||
                    !furnituresByCollection.Any()) continue;
                
                var spawnedTag = Instantiate(furnitureTagComponentPrefab, furnitureTagContent);
                spawnedTag.Initialize(furnitureCollectionType, ScrollToElement);

                var spawnedTitleCollection = Instantiate(furnitureCollectionTitleComponentPrefab, furnitureEntriesContent);
                spawnedTitleCollection.Initialize(new FurnitureCollectionTitleDto {Collection = furnitureCollectionType});
                _titleComponents.Add(spawnedTitleCollection);

                var newGridContainer = Instantiate(gridContainerComponentPrefab, furnitureEntriesContent);
                foreach (var furniture in furnituresByCollection.Where(furniture => furniture.AvailableInShop))
                {
                    newGridContainer.InstantiateGridComponent(furnitureEntryComponentPrefab,
                        new GridEntryDto
                        {
                            Sprite = furniture.Sprite,
                            Title = furniture.Name,
                            Description = furniture.Description,
                            Quantity = furniture.Price,
                            OnClick = () => OnFurnitureSelected?.Invoke(furniture)
                        });
                }
                _gridContainerComponents.Add(newGridContainer);
            }
        }

        private void ScrollToElement(FurnitureCollection furnitureCollection)
        {
            var titleElement = _titleComponents.FirstOrDefault(title => title.GetFurnitureCollection() == furnitureCollection);
            if (titleElement is null)
                return;
            scrollToElement.ScrollTo(titleElement.GetComponent<RectTransform>());
        }
    }
}