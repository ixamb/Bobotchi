using UnityEngine.Events;
using UnityEngine.UI;

namespace Core.Extensions
{
    public static class ButtonExtensions
    {
        public static void ReplaceListeners(this Button.ButtonClickedEvent buttonClickedEvent, UnityAction call)
        {
            buttonClickedEvent.RemoveAllListeners();
            buttonClickedEvent.AddListener(call);
        }
    }
}