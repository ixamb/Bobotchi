using System;
using BB.UI.Inventory.Views;
using Core.Extensions;
using Core.Services.Views;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.Inventory
{
    public class InventoryViewContext : MonoBehaviour
    {
        [SerializeField] private View inventoryView;
        [SerializeField] private InventoryListView inventoryListView;
        [SerializeField] private InventoryDetailView inventoryDetailView;
        [SerializeField] private Button navigateBackButton;
        
        private InventoryViewCoordinator _coordinator;

        private void Awake()
        {
            _coordinator = new InventoryViewCoordinator(inventoryListView, inventoryDetailView, Exit);
            navigateBackButton.onClick.ReplaceListeners(() => _coordinator.NavigateBack());
        }
        
        private void Exit()
        {
            inventoryView.HideView();
        }
    }
}