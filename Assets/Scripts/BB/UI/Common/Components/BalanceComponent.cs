using TMPro;
using UnityEngine;

namespace BB.UI.Common.Components
{
    public sealed class BalanceComponent : MonoBehaviour
    {
        [SerializeField] private TMP_Text commonBalanceText;
        
        public void UpdateComponent(float newBalance)
        {
            commonBalanceText.text = $"{newBalance}€";
        }
    }
}