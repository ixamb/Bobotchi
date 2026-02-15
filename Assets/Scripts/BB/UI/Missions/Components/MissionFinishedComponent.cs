using System;
using System.Collections.Generic;
using BB.Services.Missions;
using Core.Runtime.Extensions;
using Core.Runtime.Services.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.Missions.Components
{
    public class MissionFinishedComponent : ViewComponent<MissionFinishedDto>
    {
        [SerializeField] private TMP_Text title;
        [Space]
        [SerializeField] private MissionRewardEntryComponent rewardEntryComponentPrefab;
        [SerializeField] private Transform rewardParent;
        [Space]
        [SerializeField] private Button finishedButton;

        private readonly List<MissionRewardEntryComponent> _rewardEntryComponents = new();
        
        public Action OnFinishClick { get; set; }
        
        public override void Initialize(MissionFinishedDto missionFinishedDto)
        {
            _rewardEntryComponents.DestroyAndClear();
            
            title.text = missionFinishedDto.Mission.name;
            foreach (var reward in missionFinishedDto.Mission.EndMissionActions)
            {
                var spawnedRewardEntry = Instantiate(rewardEntryComponentPrefab, rewardParent);
                spawnedRewardEntry.Initialize(new MissionRewardDto { OnEndMissionAction = reward});
                _rewardEntryComponents.Add(spawnedRewardEntry);
            }
            finishedButton.onClick.ReplaceListeners(() => OnFinishClick?.Invoke());
        }
    }

    public class MissionFinishedDto : ComponentDto
    {
        public Mission Mission { get; set; }
    }
}