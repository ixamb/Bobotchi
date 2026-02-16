using BB.Management.FurniturePlacement;
using BB.Management.FurniturePlacement.Props;
using TheForge.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.PropPlacement
{
    public class PropPlacementButtonGroup : MonoBehaviour, IPropPlacementValidObserver, IPropPlacementInvalidObserver
    {
        [SerializeField] private Button placeButton;
        [SerializeField] private Button rotateButton;
        [SerializeField] private Button deleteButton;
        [SerializeField] private Button cancelButton;

        private void Start()
        {
            FurniturePlacementManager.Instance.RegisterValidPlacementObserver(this);
            FurniturePlacementManager.Instance.RegisterInvalidPlacementObserver(this);
            
            placeButton.onClick.ReplaceListeners(() => FurniturePlacementManager.Instance.PlaceProp());
            rotateButton.onClick.ReplaceListeners(() => FurniturePlacementManager.Instance.RotateProp());
            deleteButton.onClick.ReplaceListeners(() => FurniturePlacementManager.Instance.DeleteProp());
            cancelButton.onClick.ReplaceListeners(() => FurniturePlacementManager.Instance.CancelPropPlacement());
        }

        public void OnPropPlacementValid()
        {
            placeButton.interactable = true;
        }

        public void OnPropPlacementInvalid()
        {
            placeButton.interactable = false;
        }
    }
}