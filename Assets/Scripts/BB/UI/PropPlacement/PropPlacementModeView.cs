using BB.Entities;
using BB.Management.FurniturePlacement;
using BB.Management.FurniturePlacement.Props;
using Core.Runtime.Extensions;
using Core.Runtime.Services.Views;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.PropPlacement
{
    public class PropPlacementModeView : View, IPropPlacementModeObserver
    {
        [Space]
        [SerializeField] private Button triggerButton;
        [SerializeField] private GameObject furnitureInventoryButton;
        [SerializeField] private PropPlacementButtonGroup controlButtonsParent;

        private bool _isEditModeActive;
        
        private void Start()
        {
            triggerButton.onClick.ReplaceListeners(() =>
            {
                if (_isEditModeActive)
                    DeactivatePropEditMode();
                else
                    ActivatePropEditMode();
            });
            FurniturePlacementManager.Instance.RegisterModeObserver(this);
        }

        private void ActivatePropEditMode()
        {
            FurniturePlacementManager.Instance.Activate();
            UpdateControlsVisibility(PropPlacementMode.Overview);
            _isEditModeActive = true;
        }

        public void ActivateEditModeWithPropSelected(Prop prop)
        {
            FurniturePlacementManager.Instance.InitializePropPlacementFromInventory(prop);
            UpdateControlsVisibility(PropPlacementMode.Overview);
            
            if(!_isEditModeActive)
                triggerButton.animator.SetTrigger("Pressed"); // we force the button animation since the new state is triggered from outside
            
            _isEditModeActive = true;
        }

        private void DeactivatePropEditMode()
        {
            FurniturePlacementManager.Instance.Deactivate();
            UpdateControlsVisibility(PropPlacementMode.Disabled);
            _isEditModeActive = false;
        }

        private void UpdateControlsVisibility(PropPlacementMode newPlacementMode)
        {
            furnitureInventoryButton.gameObject.SetActive(newPlacementMode == PropPlacementMode.Overview);
            controlButtonsParent.gameObject.SetActive(newPlacementMode == PropPlacementMode.Placement);
        }

        public void OnPlacementModeChanged(PropPlacementMode newMode)
        {
            UpdateControlsVisibility(newMode);
        }
    }
}