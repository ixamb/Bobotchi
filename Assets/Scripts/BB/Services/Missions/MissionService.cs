using System;
using System.Collections.Generic;
using System.Linq;
using BB.Actions;
using BB.Services.Modules.LocalSave;
using Core.Services;
using JetBrains.Annotations;
using UnityEngine;

namespace BB.Services.Missions
{
    public sealed class MissionService : Singleton<MissionService, IMissionService>, IMissionService
    {
        [SerializeField] private List<Mission> missions;
        
        protected override void Init()
        { }

        public void LaunchMission(Mission mission)
        {
            BBLocalSaveService.Instance.RunningMission.Register(mission, autoSave: true);
        }

        public bool IsMissionRunning()
        {
            return BBLocalSaveService.Instance.RunningMission.IsRunning();
        }

        public bool IsMissionFinished()
        {
            return BBLocalSaveService.Instance.RunningMission.GetRunningMissionDto().EndTime <= DateTime.Now;
        }

        public bool IsMissionLaunchable(Mission mission)
        {
            foreach (var missionEffect in mission.EndMissionActions)
            {
                if (missionEffect is StateStatEffectAction action)
                {
                    if (action.Affection < 0)
                    {
                        if (BBLocalSaveService.Instance.StateStat.Get(action.AffectedState) < action.Affection)
                            return false;
                    }
                }
            }

            return true;
        }

        [CanBeNull]
        public Mission GetRunningMissionDefinition()
        {
            var runningMission = BBLocalSaveService.Instance.RunningMission.GetRunningMissionDto();
            var missionDefinition = missions.FirstOrDefault(mission => mission.Guid == runningMission.MissionGuid);
            return missionDefinition;
        }

        public DateTime? GetRunningMissionStartDate()
        {
            return BBLocalSaveService.Instance.RunningMission.GetRunningMissionDto()?.StartTime; 
        }

        public DateTime? GetRunningMissionEndDate()
        {
            return BBLocalSaveService.Instance.RunningMission.GetRunningMissionDto()?.EndTime;
        }

        public void CompleteRunningMission()
        {
            var runningMission = BBLocalSaveService.Instance.RunningMission.GetRunningMissionDto();
            var missionDefinition = missions.FirstOrDefault(mission => mission.Guid == runningMission.MissionGuid);
            missionDefinition?.EndMissionActions.ForEach(action => action.Execute());
            BBLocalSaveService.Instance.RunningMission.Clear(autoSave: true);
        }
        
        public List<Mission> Missions() => missions;
    }
}