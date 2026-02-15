using BB.UI.Inventory.Views;
using Core.Runtime.Services.Views;
using Core.Runtime.Systems.Actions;
using UnityEngine;

namespace BB.Actions
{
    [CreateAssetMenu(fileName = "Show Inventory Action", menuName = "BB/Actions/Show Inventory Action")]
    public class ShowInventoryAction : GameAction
    {
        [SerializeField] private InventoryListView.InventoryViewMode defaultTab;

        protected override void Executable()
        {
            var view = ViewService.Instance.GetView<View>("inventory-view");
            view.GetComponentInChildren<InventoryListView>().Initialize(defaultTab);
            view.ShowView();
        }
    }
}