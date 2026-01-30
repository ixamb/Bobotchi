using System;
using System.Collections.Generic;
using System.Linq;
using BB.Services.Missions;
using BB.UI.Missions.Components;
using Core.Extensions;
using Core.Services.Views;
using UnityEngine;

namespace BB.UI.Missions
{
    public class MissionListComponent : ViewComponent<MissionListDto>
    {
        [SerializeField] private MissionEntryComponent missionEntryComponentPrefab;
        [SerializeField] private Transform missionEntryContainer;
        [SerializeField] private GameObject missionEntrySeparator;
        [Space]
        [SerializeField] private GameObject noMission;
        
        private readonly List<MissionEntryComponent> _missionEntryComponents = new();
        private readonly List<GameObject> _separators = new();
        
        public override void Initialize(MissionListDto missionListDto)
        {
            _missionEntryComponents.DestroyAndClear();
            _separators.DestroyAndClear();
            
            foreach (var mission in missionListDto.LaunchableMissionsMap)
            {
                var missionEntry = Instantiate(missionEntryComponentPrefab, missionEntryContainer);
                missionEntry.Initialize(new MissionEntryDto
                {
                    Mission = mission.Key,
                    Launchable = mission.Value,
                    LaunchAction = () => missionListDto.LaunchAction?.Invoke(mission.Key)
                });
                _missionEntryComponents.Add(missionEntry);
                _separators.Add(Instantiate(missionEntrySeparator, missionEntryContainer));
            }
            
            noMission.SetActive(!_missionEntryComponents.Any());
        }
    }

    public class MissionListDto : ComponentDto
    {
        public Dictionary<Mission, bool> LaunchableMissionsMap { get; set; }
        public Action<Mission> LaunchAction { get; set; }
    }
}