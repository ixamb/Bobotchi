using UnityEngine;

namespace BB.UI.Inventory.Components
{
    public class InventoryTabButtonComponent : MonoBehaviour
    {

        public void OnSelected()
        {
            transform.localScale = Vector3.one * 1.2f;
        }

        public void OnUnselected()
        {
            transform.localScale = Vector3.one;
        }
    }
}