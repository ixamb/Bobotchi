using BB.UI.Common.Components;
using TheForge.Services.Views;
using UnityEngine;

namespace BB.UI
{
    public class CharacterStateStatView : View
    {
        [Space]
        [SerializeField] private StatBarExpandedComponent hungerBar;
        [SerializeField] private StatBarExpandedComponent energyBar;
        [SerializeField] private StatBarExpandedComponent esteemBar;

        public void UpdateStatBars()
        {
            hungerBar.UpdateProgressBar();
            energyBar.UpdateProgressBar();
            esteemBar.UpdateProgressBar();
        }
    }
}