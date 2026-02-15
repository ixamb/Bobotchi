using System;
using BB.Services.Missions;
using Core.Runtime.Extensions;
using Core.Runtime.Services.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.Missions.Components
{
    public class MissionEntryComponent : ViewComponent<MissionEntryDto>
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        [Space]
        [SerializeField] private MissionRewardEntryComponent rewardEntryComponentPrefab;
        [SerializeField] private Transform rewardParent;
        [Space]
        [SerializeField] private Button launchButton;

        public override void Initialize(MissionEntryDto missionEntryDto)
        {
            title.text = missionEntryDto.Mission.Title;
            description.text = $"pendant : {missionEntryDto.Mission.Duration}";

            foreach (var reward in missionEntryDto.Mission.EndMissionActions)
            {
                var spawnedRewardEntry = Instantiate(rewardEntryComponentPrefab, rewardParent);
                spawnedRewardEntry.Initialize(new MissionRewardDto { OnEndMissionAction = reward });
            }
            
            launchButton.onClick.ReplaceListeners(() => missionEntryDto.LaunchAction?.Invoke());
            launchButton.interactable = missionEntryDto.Launchable;
        }
    }

    public class MissionEntryDto : ComponentDto
    {
        public Mission Mission { get; set; }
        public bool Launchable { get; set; }
        public Action LaunchAction { get; set; }
    }
}