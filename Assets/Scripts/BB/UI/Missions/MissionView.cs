using System.Linq;
using BB.Services.Missions;
using BB.UI.Missions.Components;
using Core.Services.Views;
using UnityEngine;

namespace BB.UI.Missions
{
    public class MissionView : View
    {
        [SerializeField] private MissionListComponent missionListComponent;
        [SerializeField] private MissionRunningComponent missionRunningComponent;
        [SerializeField] private MissionFinishedComponent missionFinishedComponent;

        private void Start()
        {
            OnShow += Initialize;
        }

        private void Initialize()
        {
            missionRunningComponent.OnEndDateReached = () => ShowMissionFinished(RunningMission);
            missionFinishedComponent.OnFinishClick = () =>
            {
                MissionService.Instance.CompleteRunningMission();
                HideView();
            };
            
            if (!MissionService.Instance.IsMissionRunning())
            {
                ShowMissionList();
                return;
            }
            
            
            if (!MissionService.Instance.IsMissionFinished())
            {
                ShowMissionRunning(RunningMission);
                return;
            }
            
            ShowMissionFinished(RunningMission);
        }

        private void ShowMissionList()
        {
            missionListComponent.Initialize(new MissionListDto
            {
                LaunchableMissionsMap = MissionService.Instance.Missions().ToDictionary(mission => mission,
                    mission => MissionService.Instance.IsMissionLaunchable(mission)),
                LaunchAction = mission =>
                {
                    MissionService.Instance.LaunchMission(mission);
                    ShowMissionRunning(mission);
                }
            });
            
            missionListComponent.gameObject.SetActive(true);
            missionRunningComponent.gameObject.SetActive(false);
            missionFinishedComponent.gameObject.SetActive(false);
        }

        private void ShowMissionRunning(Mission mission)
        {
            missionRunningComponent.Initialize(new MissionRunningDto
            {
                Mission = mission,
                StartDate = MissionService.Instance.GetRunningMissionStartDate()!.Value,
                EndDate = MissionService.Instance.GetRunningMissionEndDate()!.Value,
            });
            
            missionListComponent.gameObject.SetActive(false);
            missionRunningComponent.gameObject.SetActive(true);
            missionFinishedComponent.gameObject.SetActive(false);
        }

        private void ShowMissionFinished(Mission mission)
        {
            missionFinishedComponent.Initialize(new MissionFinishedDto { Mission = mission });
            missionListComponent.gameObject.SetActive(false);
            missionRunningComponent.gameObject.SetActive(false);
            missionFinishedComponent.gameObject.SetActive(true);
        }
        
        private Mission RunningMission => MissionService.Instance.GetRunningMissionDefinition();
    }
}