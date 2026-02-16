using BB.Actions;
using TheForge.Services.Views;
using TheForge.Systems.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.Missions.Components
{
    public class MissionRewardEntryComponent : ViewComponent<MissionRewardDto>
    {
        [SerializeField] private Image logo;
        [SerializeField] private TMP_Text counter;

        public override void Initialize(MissionRewardDto missionRewardDto)
        {
            if (missionRewardDto.OnEndMissionAction is StateStatEffectAction stateStatEffectAction)
            {
                counter.text = stateStatEffectAction.AffectedState.ToString();
            }

            if (missionRewardDto.OnEndMissionAction is CurrencyEffectAction currencyEffectAction)
            {
                var quantity = currencyEffectAction.QuantityToImpact;
                counter.text = $"{(quantity >= 0 ? "+" : string.Empty)}{quantity}€";
            }
        }
    }
    
    public class MissionRewardDto : ComponentDto
    {
        public GameAction OnEndMissionAction { get; set; }
    }
}