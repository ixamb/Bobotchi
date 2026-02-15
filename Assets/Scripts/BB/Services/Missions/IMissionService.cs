using System;
using System.Collections.Generic;
using Core.Runtime.Services;
using JetBrains.Annotations;

namespace BB.Services.Missions
{
    public interface IMissionService : ISingleton
    {
        void LaunchMission(Mission mission);
        bool IsMissionRunning();
        bool IsMissionFinished();
        bool IsMissionLaunchable(Mission mission);
        [CanBeNull] Mission GetRunningMissionDefinition();
        DateTime? GetRunningMissionStartDate();
        DateTime? GetRunningMissionEndDate();
        void CompleteRunningMission();
        List<Mission> Missions();
    }
}