using BB.Data;
using BB.Services.Modules.LocalSave;
using BB.UI.Main;
using TheForge.Services.Views;
using TheForge.Systems.Actions;
using UnityEngine;

namespace BB.Actions
{
    [CreateAssetMenu(fileName = "Currency Effect Action", menuName = "BB/Actions/Currency Effect Action")]
    public sealed class CurrencyEffectAction : GameAction
    {
        [SerializeField] private Currency currency;
        [SerializeField] private float quantityToImpact;
        
        protected override void Executable()
        {
            BBLocalSaveService.Instance.Balance.Update(currency, quantityToImpact);
            ViewService.Instance.GetView<StatusBarView>("status-bar-view").UpdateView();
        }
        
        public Currency Currency => currency;
        public float QuantityToImpact => quantityToImpact;
    }
}