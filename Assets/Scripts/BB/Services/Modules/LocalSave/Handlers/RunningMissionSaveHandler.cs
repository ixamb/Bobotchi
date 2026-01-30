using System;
using BB.Data;
using BB.Services.Missions;

namespace BB.Services.Modules.LocalSave.Handlers
{
    public sealed class RunningMissionSaveHandler : SaveHandler<RunningMissionDto>
    {
        protected override string ConstantEntryKey => Constants.LocalSaveEntries.RunningMission;
        
        public void Register(Mission mission, bool autoSave = false)
        {
            var runningMissionDto = new RunningMissionDto
            {
                MissionGuid = mission.Guid,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now
                    .AddHours(mission.Duration.Hours)
                    .AddMinutes(mission.Duration.Minutes)
                    .AddSeconds(mission.Duration.Seconds)
            };
            SetData(runningMissionDto, autoSave);
        }
        
        public bool IsRunning()
        {
            var runningMissionDto = GetRunningMissionDto();
            return
                runningMissionDto.MissionGuid is not null
                && runningMissionDto.StartTime is not null
                && runningMissionDto.EndTime is not null;
        }

        public RunningMissionDto GetRunningMissionDto()
        {
            return GetData();
        }

        public void Clear(bool autoSave = false)
        {
            SetData(new RunningMissionDto(), autoSave);
        }
    }
}