using System;
using System.Collections.Generic;
using System.Linq;
using BB.Data;
using BB.Grid;
using BB.Grid.Tiles;
using BB.Management.FurniturePlacement.Props.Utils;
using BB.Services.Modules.GameData;
using BB.Services.Modules.LocalSave;
using Vector2Int = TheForge.Types.Vector2Int;

namespace BB.Management.FurniturePlacement.Props
{
    public interface IPropPlacementService
    {
        PropObject SpawnAndPlaceOnTile(Entities.Prop prop, Tile tile, PropPlacementDirection direction);
        void MoveProp(PropObject propObject, Tile targetTile);
        void SavePlacement(PropObject propObject, Entities.Prop prop, Tile tile);
        void DeleteProp(PropObject propObject, bool retrieveIntoInventory = true);
        bool IsPlacementValidOnTile(PropObject propObject, Tile tile);
        void LoadSavedProps();

        void RegisterPlacementObserver(IPropPlacementObserver placementObserver);
        void UnregisterPlacementObserver(IPropPlacementObserver placementObserver);
        void RegisterRemoveObserver(IPropRemoveObserver removeObserver);
        void UnregisterRemoveObserver(IPropRemoveObserver removeObserver);

    }
    
    public sealed class PropPlacementService : IPropPlacementService
    {
        private readonly IPropPlacementRepository _repository;
        private readonly IPropPlacementVisualizer _visualizer;

        private readonly List<IPropPlacementObserver> _placementObservers = new();
        private readonly List<IPropRemoveObserver> _removeObservers = new();
        
        public PropPlacementService(
            IPropPlacementRepository repository,
            IPropPlacementVisualizer visualizer)
        {
            _repository = repository;
            _visualizer = visualizer;
        }

        public PropObject SpawnAndPlaceOnTile(Entities.Prop prop, Tile tile, PropPlacementDirection direction)
        {
            var propObject = _visualizer.SpawnProp(prop);
            propObject.PropCategory = prop.Category;
            _visualizer.PlaceOnTile(propObject, tile, direction);
            propObject.UpdateSortingOrder(prop.Size, new Vector2Int(0, GridManager.Instance.GetGridSize().Y));
            return propObject;
        }

        public void MoveProp(PropObject propObject, Tile targetTile)
        {
            _visualizer.PlaceOnTile(propObject, targetTile, propObject.Rotation());
            propObject.UpdateSortingOrder(_repository.TryGetProp(propObject, out var prop)
                ? prop.Size
                : Vector2Int.One, new Vector2Int(0, GridManager.Instance.GetGridSize().Y));
        }

        public void SavePlacement(PropObject propObject, Entities.Prop prop, Tile tile)
        {
            if (propObject.PlacementGuid == Guid.Empty)
                propObject.PlacementGuid = Guid.NewGuid();
            
            _repository.RegisterPlacedProp(propObject);
            foreach (var placementObserver in _placementObservers)
                placementObserver.OnPropPlaced(prop, propObject, tile);
        }

        public void DeleteProp(PropObject propObject, bool retrieveIntoInventory = true)
        {
            _repository.UnregisterPlacedProp(propObject);
            _visualizer.DestroyProp(propObject);

            if (_repository.TryGetProp(propObject.Guid, out var furniture))
            {
                foreach (var placementObserver in _removeObservers)
                    placementObserver.OnPropRemoved(furniture,  propObject, retrieveIntoInventory);
            }
        }
        
        public void LoadSavedProps()
        {
            foreach (var placement in BBLocalSaveService.Instance.FurniturePlacement.GetAllPlaced())
            {
                var tile = GridManager.Instance.GetTile(new Vector2Int(placement.GridX, placement.GridY));
                if (tile == null) continue;

                var prop = GameDataService.Instance.GetProp(placement.PropGuid);
                if (prop == null) continue;

                var propObject = SpawnAndPlaceOnTile(prop, tile, placement.Direction);
                propObject.PlacementGuid = placement.PlacementEntryGuid;
                _repository.RegisterPlacedProp(propObject);
            }
        }

        public bool IsPlacementValidOnTile(PropObject propObject, Tile tile)
        {
            if (!_repository.TryGetProp(propObject, out var furniture))
                return false;
            
            var possiblyOccupiedTilesByTouchedProp = PropPlacementUtils.GetTilesOccupiedByProp(furniture, propObject).ToList();
            if (possiblyOccupiedTilesByTouchedProp.Any(possibleTile => possibleTile is null))
                return false;
            
            var propsWithinRadius = PropPlacementUtils.GetPropsWithinRadius(
                baseTile: tile,
                radius: Constants.FurnitureDimension.XMaximum,
                propCategory: furniture.Category).ToList();
            propsWithinRadius.Remove(propObject);
            if (!propsWithinRadius.Any())
                return true;
            
            var occupiedTilesWithinRadius = propsWithinRadius.SelectMany(propWithinRadius
                => _repository.TryGetProp(propObject, out var propWithinRadiusDefinition) ?
                    PropPlacementUtils.GetTilesOccupiedByProp(propWithinRadiusDefinition, propWithinRadius)
                    : null).ToHashSet();
            
            return possiblyOccupiedTilesByTouchedProp.All(possiblyOccupiedTile => !occupiedTilesWithinRadius.Contains(possiblyOccupiedTile));
        }


        
        public void RegisterPlacementObserver(IPropPlacementObserver placementObserver)
        {
            if (!_placementObservers.Contains(placementObserver))
                _placementObservers.Add(placementObserver);
        }

        public void UnregisterPlacementObserver(IPropPlacementObserver placementObserver)
        {
            if (_placementObservers.Contains(placementObserver))
                _placementObservers.Remove(placementObserver);
        }

        public void RegisterRemoveObserver(IPropRemoveObserver removeObserver)
        {
            if (!_removeObservers.Contains(removeObserver))
                _removeObservers.Add(removeObserver);
        }

        public void UnregisterRemoveObserver(IPropRemoveObserver removeObserver)
        {
            if (_removeObservers.Contains(removeObserver))
                _removeObservers.Remove(removeObserver);
        }
    }
}