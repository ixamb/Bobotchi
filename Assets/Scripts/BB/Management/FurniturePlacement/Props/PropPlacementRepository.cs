using System;
using System.Collections.Generic;
using System.Linq;
using BB.Services.Modules.GameData;

namespace BB.Management.FurniturePlacement.Props
{
    public interface IPropPlacementRepository
    {
        bool TryGetProp(Guid propObjectGuid, out Entities.Prop prop);
        bool TryGetProp(PropObject propObject, out Entities.Prop prop);
        IReadOnlyList<PropObject> GetPlacedProps();
        void RegisterPlacedProp(PropObject propObject);
        void UnregisterPlacedProp(PropObject propObject);
    }
    
    public sealed class PropPlacementRepository : IPropPlacementRepository
    {
        private readonly Dictionary<Guid, Entities.Prop> _propMap;
        private readonly List<PropObject> _placedProps = new();

        public PropPlacementRepository()
        {
            _propMap = GameDataService.Instance
                .GetProps()
                .Where(f => f.PropPrefab != null)
                .ToDictionary(f => f.PropPrefab.Guid, f => f);
        }

        public bool TryGetProp(Guid propObjectGuid, out Entities.Prop prop)
            => _propMap.TryGetValue(propObjectGuid, out prop);

        public bool TryGetProp(PropObject propObject, out Entities.Prop prop)
            => _propMap.TryGetValue(propObject.Guid, out prop);
        
        public IReadOnlyList<PropObject> GetPlacedProps() => _placedProps;
        
        public void RegisterPlacedProp(PropObject propObject)
        {
            if (!_placedProps.Contains(propObject))
                _placedProps.Add(propObject);
        }

        public void UnregisterPlacedProp(PropObject propObject)
        {
            _placedProps.Remove(propObject);
        }
    }
}