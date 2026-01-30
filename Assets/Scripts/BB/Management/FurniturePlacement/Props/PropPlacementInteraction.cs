using System.Collections.Generic;
using BB.Grid.Tiles;
using Core.Extensions;
using Core.Mechanics.Input.ObjectTouch;
using UnityEngine;

namespace BB.Management.FurniturePlacement.Props
{
    [RequireComponent(typeof(ObjectTouchHandler))]
    public sealed class PropPlacementInteraction : MonoBehaviour, IObjectTouchReleasedHandler
    {
        private IPropPlacementStateMachine _stateMachine;
        private IPropPlacementService _service;
        private IPropPlacementRepository _repository;
        
        private PropPlacementContext _context;

        private readonly List<IPropSelectedObserver> _propSelectedObservers = new();
        private readonly List<IPropPlacementValidObserver> _placementValidObservers = new();
        private readonly List<IPropPlacementInvalidObserver> _placementInvalidObservers = new();
        
        public void Initialize(IPropPlacementStateMachine stateMachine, IPropPlacementService service, IPropPlacementRepository repository, PropPlacementContext context)
        {
            _stateMachine = stateMachine;
            _service = service;
            _context = context;
            _repository = repository;
        }

        public void OnObjectTouchReleased(GameObject touchedObject)
        {
            if (_stateMachine.CurrentMode() == PropPlacementMode.Disabled || _context == null)
                return;

            if (touchedObject.TryGetComponent(out Tile tile))
            {
                HandleTileTouch(tile);
            }
            else if (touchedObject.transform.TryGetComponent(out PropObject furnitureObject))
            {
                HandlePropTouch(furnitureObject);
            }
        }

        private void HandleTileTouch(Tile tile)
        {
            if (_stateMachine.CurrentMode() == PropPlacementMode.Overview)
            {
                if (_context.IsFromInventory)
                {
                    var prop = GetPropFromContext();
                    const PropPlacementDirection baseRotation = PropPlacementDirection.Left;
                    var spawned = _service.SpawnAndPlaceOnTile(prop, tile, baseRotation);
                    _context.UpdateFromScene(spawned.gameObject, null, baseRotation);
                    _stateMachine.ChangeMode(PropPlacementMode.Placement);
                    spawned.OnObjectSelected();
                    _propSelectedObservers.ForEach(observer => observer.OnPropSelected(spawned));
                }
                else
                {
                    return;
                }
            }
            
            else if (_stateMachine.CurrentMode() == PropPlacementMode.Placement && !_context.IsFromInventory)
            {
                _service.MoveProp(_context.PropObject, tile);
            }
            
            if (!_service.IsPlacementValidOnTile(_context.PropObject, tile))
            {
                _context.PropObject.OnObjectInvalid();
                foreach (var observer in _placementInvalidObservers)
                    observer.OnPropPlacementInvalid();
            }
            else
            {
                _context.PropObject.OnObjectValid();
                foreach (var observer in _placementValidObservers)
                    observer.OnPropPlacementValid();
            }
        }

        private void HandlePropTouch(PropObject propObject)
        {
            if (_stateMachine.CanSelectProp())
            {
                if (!propObject.gameObject.transform.TryGetComponentInParent<Tile>(out var sourceTile))
                    return;
                _context.UpdateFromScene(propObject.gameObject, sourceTile, propObject.Rotation());
                _stateMachine.ChangeMode(PropPlacementMode.Placement);
                propObject.OnObjectSelected();
                _propSelectedObservers.ForEach(observer => observer.OnPropSelected(propObject));
            }
        }

        public void HandleRotation()
        {
            if (_stateMachine.CanPlaceProp())
                _context?.PropObject?.Rotate();

            if (!_repository.TryGetProp(_context?.PropObject, out _))
                return;

            if (!_service.IsPlacementValidOnTile(_context?.PropObject, _context!.PropObject!.TryGetParentTile(out var tile) ? tile : null))
            //if (FurniturePlacementUtils.GetTilesOccupiedByFurniture(furniture, _context?.FurnitureObject).Any(tile => tile is null || tile.State != TileState.Free))
            {
                foreach (var observer in _placementInvalidObservers)
                    observer.OnPropPlacementInvalid();
                _context?.PropObject?.OnObjectInvalid();
                return;
            }
            
            foreach (var observer in _placementValidObservers)
                observer.OnPropPlacementValid();
            _context?.PropObject?.OnObjectValid();
        }

        private Entities.Prop GetPropFromContext()
        {
            return _repository.TryGetProp(_context.PropObject.Guid, out var prop) ? prop : null;
        }
        
        public void RegisterPropSelectedObserver(IPropSelectedObserver observer)
        {
            if (!_propSelectedObservers.Contains(observer))
                _propSelectedObservers.Add(observer);
        }

        public void UnregisterPropSelectedObserver(IPropSelectedObserver observer)
        {
            if (_propSelectedObservers.Contains(observer))
                _propSelectedObservers.Remove(observer);
        }

        public void RegisterPlacementValidObserver(IPropPlacementValidObserver observer)
        {
            if (!_placementValidObservers.Contains(observer))
                _placementValidObservers.Add(observer);
        }

        public void UnregisterPlacementValidObserver(IPropPlacementValidObserver observer)
        {
            if (_placementValidObservers.Contains(observer))
                _placementValidObservers.Remove(observer);
        }
        
        public void RegisterPlacementInvalidObserver(IPropPlacementInvalidObserver observer)
        {
            if (!_placementInvalidObservers.Contains(observer))
                _placementInvalidObservers.Add(observer);
        }

        public void UnregisterPlacementInvalidObserver(IPropPlacementInvalidObserver observer)
        {
            if (_placementInvalidObservers.Contains(observer))
                _placementInvalidObservers.Remove(observer);
        } 
    }
}