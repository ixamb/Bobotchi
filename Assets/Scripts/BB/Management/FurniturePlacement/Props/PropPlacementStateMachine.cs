using System.Collections.Generic;

namespace BB.Management.FurniturePlacement.Props
{
    public interface IPropPlacementStateMachine
    {
        PropPlacementMode CurrentMode();
        void ChangeMode(PropPlacementMode newMode);
        bool CanPlaceProp();
        bool CanSelectProp();
        void RegisterObserver(IPropPlacementModeObserver modeObserver);
        void UnregisterObserver(IPropPlacementModeObserver modeObserver);
    }
    
    public sealed class PropPlacementStateMachine : IPropPlacementStateMachine
    {
        private readonly List<IPropPlacementModeObserver> _placementModeObservers = new();
        private PropPlacementMode _currentMode = PropPlacementMode.Disabled;

        public PropPlacementMode CurrentMode() => _currentMode;
        
        public void ChangeMode(PropPlacementMode newMode)
        {
            if (_currentMode == newMode)
                return;
            
            _currentMode = newMode;
            NotifyObservers();
        }

        public bool CanPlaceProp() => _currentMode == PropPlacementMode.Placement;
        public bool CanSelectProp() => _currentMode == PropPlacementMode.Overview;
    
        public void RegisterObserver(IPropPlacementModeObserver modeObserver)
        {
            if (!_placementModeObservers.Contains(modeObserver))
                _placementModeObservers.Add(modeObserver);
        }

        public void UnregisterObserver(IPropPlacementModeObserver modeObserver)
        {
            _placementModeObservers.Remove(modeObserver);
        }
    
        private void NotifyObservers()
        {
            foreach (var observer in _placementModeObservers)
                observer.OnPlacementModeChanged(_currentMode);
        }
    }
}