using BB.Data;
using BB.Services.Modules.LocalSave;
using BB.UI.Common.Components;
using TheForge.Extensions;
using TheForge.Services.Views;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.Main
{
    public sealed class StatusBarView : View
    {
        [SerializeField] private BalanceComponent balanceComponent;
        [SerializeField] private Button stateStatBarButton;
        
        private void Start()
        {
            UpdateView();
            
            stateStatBarButton.onClick.ReplaceListeners(() =>
            {
                var stateStatView = ViewService.Instance.GetView<CharacterStateStatView>("character-state-stat-view");
                if (stateStatView.IsVisibleAndActive())
                {
                    stateStatView.HideView();
                }
                else
                {
                    stateStatView.UpdateStatBars();
                    stateStatView.ShowView();
                }

            });
        }

        public void UpdateView()
        {
            balanceComponent.UpdateComponent(BBLocalSaveService.Instance.Balance.Get(Currency.Common));
        }
    }
}