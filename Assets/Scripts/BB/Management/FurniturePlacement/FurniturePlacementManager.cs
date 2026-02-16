using System.Collections.Generic;
using BB.Entities;
using BB.Management.FurniturePlacement.Props;
using BB.Management.FurniturePlacement.Surfaces;
using TheForge.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace BB.Management.FurniturePlacement
{
    [RequireComponent(typeof(PropPlacementInteraction))]
    public sealed class FurniturePlacementManager : Singleton<FurniturePlacementManager, IFurniturePlacementManager>, IFurniturePlacementManager
    {
        private IPropPlacementStateMachine _propPlacementStateMachine;
        private IPropPlacementService _propPlacementService;
        private IPropPlacementRepository _propPlacementRepository;
        private IPropPlacementVisualizer _propPlacementVisualizer;
        private PropPlacementContext _propPlacementCurrentContext;
        private PropPlacementInteraction propPlacementInteraction;
        
        private ISurfacePlacementService _surfacePlacementService;
        
        private readonly List<IPropUnselectedObserver> _propUnselectedObservers = new();
        
        protected override void Init()
        {
            _propPlacementRepository = new PropPlacementRepository();
            _propPlacementVisualizer = new PropPlacementVisualizer();
            _propPlacementService = new PropPlacementService(_propPlacementRepository, _propPlacementVisualizer);
            _propPlacementStateMachine = new PropPlacementStateMachine();
            _propPlacementCurrentContext = new PropPlacementContext();
            
            propPlacementInteraction = GetComponent<PropPlacementInteraction>();
            propPlacementInteraction.Initialize(_propPlacementStateMachine, _propPlacementService, _propPlacementRepository, _propPlacementCurrentContext);
            _propPlacementService.LoadSavedProps();
            
            _surfacePlacementService = new SurfacePlacementService();
            _surfacePlacementService.LoadPlacedSurfaces();
        }

        public void Activate()
        {
            _propPlacementStateMachine.ChangeMode(PropPlacementMode.Overview);
        }

        public void Deactivate()
        {
            if (!_propPlacementCurrentContext.IsFromInventory)
            {
                _propPlacementCurrentContext?.PropObject?.OnObjectDeselected();
                _propUnselectedObservers.ForEach(observer => observer.OnPropUnselected(_propPlacementCurrentContext?.PropObject));
            }
            _propPlacementStateMachine.ChangeMode(PropPlacementMode.Disabled);
        }

        public void InitializePropPlacementFromInventory(Prop prop)
        {
            _propPlacementCurrentContext.UpdateFromInventory(prop);
            _propPlacementStateMachine.ChangeMode(PropPlacementMode.Overview);
        }

        public void PlaceProp()
        {
            if (!_propPlacementStateMachine.CanPlaceProp() || _propPlacementCurrentContext == null)
                return;

            if (!_propPlacementRepository.TryGetProp(_propPlacementCurrentContext.PropObject.Guid, out var prop))
                return;

            if (!_propPlacementCurrentContext.PropObject.TryGetParentTile(out var tile))
                return;

            _propPlacementService.SavePlacement(_propPlacementCurrentContext.PropObject, prop, tile);
            _propPlacementCurrentContext.PropObject.OnObjectDeselected();
            _propUnselectedObservers.ForEach(observer => observer.OnPropUnselected(_propPlacementCurrentContext?.PropObject));
            _propPlacementStateMachine.ChangeMode(PropPlacementMode.Overview);
        }

        public void RotateProp()
        {
            propPlacementInteraction.HandleRotation();
        }
        
        public void CancelPropPlacement()
        {
            if (_propPlacementCurrentContext.IsFromInventory || _propPlacementCurrentContext.OriginTile is null)
            {
                DeleteProp(retrieveIntoInventory: false);
            }
            else if(_propPlacementCurrentContext.OriginTile is not null)
            {
                _propPlacementVisualizer.PlaceOnTile(_propPlacementCurrentContext.PropObject, _propPlacementCurrentContext.OriginTile, _propPlacementCurrentContext.OriginDirection);
                _propPlacementCurrentContext.PropObject.OnObjectDeselected();
                _propUnselectedObservers.ForEach(observer => observer.OnPropUnselected(_propPlacementCurrentContext?.PropObject));
                _propPlacementStateMachine.ChangeMode(PropPlacementMode.Overview);
            }
        }

        public void DeleteProp(bool retrieveIntoInventory = true)
        {
            if (!_propPlacementStateMachine.CanPlaceProp() || _propPlacementCurrentContext == null)
                return;

            _propPlacementService.DeleteProp(_propPlacementCurrentContext.PropObject, retrieveIntoInventory);
            _propPlacementCurrentContext.Clear();
            _propPlacementStateMachine.ChangeMode(PropPlacementMode.Overview);
        }
        
        public void PlaceFloor(Surface floor) => _surfacePlacementService.PlaceSurfaceOnGround(floor);
        public void PlaceWall(Surface wall) => _surfacePlacementService.PlaceSurfaceOnWalls(wall);
        
        public void RegisterModeObserver(IPropPlacementModeObserver modeObserver) => _propPlacementStateMachine.RegisterObserver(modeObserver);
        public void UnregisterModeObserver(IPropPlacementModeObserver modeObserver) => _propPlacementStateMachine.UnregisterObserver(modeObserver);
        
        public void RegisterPropSelectedObserver(IPropSelectedObserver selectedObserver) => propPlacementInteraction.RegisterPropSelectedObserver(selectedObserver);
        public void UnregisterPropSelectedObserver(IPropSelectedObserver observer) => propPlacementInteraction.UnregisterPropSelectedObserver(observer);

        public void RegisterPropUnselectedObserver(IPropUnselectedObserver unselectedObserver)
        {
            if (!_propUnselectedObservers.Contains(unselectedObserver))
                _propUnselectedObservers.Add(unselectedObserver);
        }

        public void UnregisterPropUnselectedObserver(IPropUnselectedObserver observer)
        {
            if (_propUnselectedObservers.Contains(observer))
                _propUnselectedObservers.Remove(observer);
        }
        
        public void RegisterPlacementObserver(IPropPlacementObserver modeObserver) => _propPlacementService.RegisterPlacementObserver(modeObserver);
        public void UnregisterPlacementObserver(IPropPlacementObserver modeObserver) => _propPlacementService.UnregisterPlacementObserver(modeObserver);
        
        public void RegisterRemoveObserver(IPropRemoveObserver observer) => _propPlacementService.RegisterRemoveObserver(observer);
        public void UnregisterRemoveObserver(IPropRemoveObserver observer) => _propPlacementService.UnregisterRemoveObserver(observer);
        
        public void RegisterValidPlacementObserver(IPropPlacementValidObserver observer) => propPlacementInteraction.RegisterPlacementValidObserver(observer);
        public void UnregisterValidPlacementObserver(IPropPlacementValidObserver observer) => propPlacementInteraction.UnregisterPlacementValidObserver(observer);
        
        public void RegisterInvalidPlacementObserver(IPropPlacementInvalidObserver observer) => propPlacementInteraction.RegisterPlacementInvalidObserver(observer);
        public void UnregisterInvalidPlacementObserver(IPropPlacementInvalidObserver observer) => propPlacementInteraction.UnregisterPlacementInvalidObserver(observer);
    }
    
    public enum PropPlacementMode
    {
        /// <summary>
        /// No placement is actually possible.
        /// </summary>
        Disabled,
            
        /// <summary>
        /// The user can select prop to manipulate.
        /// </summary>
        Overview,
            
        /// <summary>
        /// The user can move/place/delete a selected prop.
        /// </summary>
        Placement,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PropPlacementDirection
    {
        Left,
        Right,
    }
}