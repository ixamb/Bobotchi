using System;
using System.Collections.Generic;
using BB.Services.Missions;
using TheForge.Extensions;
using TheForge.Services.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.Missions.Components
{
    public class MissionRunningComponent : ViewComponent<MissionRunningDto>
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private Image progressBar;
        [Space]
        [SerializeField] private MissionRewardEntryComponent rewardEntryComponentPrefab;
        [SerializeField] private Transform rewardParent;

        public Action OnEndDateReached { get; set; }

        private readonly List<MissionRewardEntryComponent> _rewardEntryComponents = new();

        private DateTime _startDate;
        private DateTime _endDate;
        private Action _onEndDateReached;
        
        private float _baseProgressBarWidth;

        private void Start()
        {
            _baseProgressBarWidth = progressBar.rectTransform.rect.width;
        }

        public override void Initialize(MissionRunningDto missionRunningDto)
        {
            _rewardEntryComponents.DestroyAndClear();
            
            title.text = missionRunningDto.Mission.Title;
            foreach (var reward in missionRunningDto.Mission.EndMissionActions)
            {
                var spawnedRewardEntry = Instantiate(rewardEntryComponentPrefab, rewardParent);
                spawnedRewardEntry.Initialize(new MissionRewardDto { OnEndMissionAction = reward });
                _rewardEntryComponents.Add(spawnedRewardEntry);
            }
            _startDate = missionRunningDto.StartDate;
            _endDate = missionRunningDto.EndDate;
        }

        private void Update()
        {
            if (!gameObject.activeSelf)
                return;

            UpdateProgressBar(GetProgressPercentage(_startDate, _endDate, DateTime.Now));
            if (_endDate <= DateTime.Now)
            {
                OnEndDateReached?.Invoke();
            }
        }
        
        private void UpdateProgressBar(float progress)
        {
            var clampedValue = Mathf.Clamp(progress, 0, 100);
            var normalizedEmpty = 1f - (clampedValue / 100);
            var offset = normalizedEmpty * _baseProgressBarWidth;
            progressBar.rectTransform.sizeDelta = new Vector2(-offset, progressBar.rectTransform.sizeDelta.y);
        }
        
        private float GetProgressPercentage(DateTime startDate, DateTime endDate, DateTime now)
        {
            var totalDuration = (endDate - startDate).TotalSeconds;
            var elapsed = (now - startDate).TotalSeconds;
            var percentage = (float)(elapsed / totalDuration * 100);
            return Mathf.Clamp(percentage, 0f, 100f);
        }
    }

    public class MissionRunningDto : ComponentDto
    {
        public Mission Mission { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}