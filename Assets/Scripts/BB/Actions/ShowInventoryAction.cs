using BB.UI.Inventory.Views;
using TheForge.Services.Views;
using TheForge.Systems.Actions;
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