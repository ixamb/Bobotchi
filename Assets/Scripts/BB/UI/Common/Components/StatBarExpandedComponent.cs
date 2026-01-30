using BB.Data;
using BB.Services.Modules.LocalSave;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.Common.Components
{
    public class StatBarExpandedComponent : MonoBehaviour
    {
        [SerializeField] private CharacterStateStat stateStat;
        [SerializeField] private Image progressBar;

        private float _baseProgressBarWidth;
        
        private void Start()
        {
            _baseProgressBarWidth = progressBar.rectTransform.rect.width;
        }

        public void UpdateProgressBar()
        {
            UpdateProgressBar(BBLocalSaveService.Instance.StateStat.Get(stateStat));
        }

        private void UpdateProgressBar(float progress)
        {
            var clampedValue = Mathf.Clamp(progress, 0, 100);
            var normalizedEmpty = 1f - (clampedValue / 100);
            var offset = normalizedEmpty * _baseProgressBarWidth;
            progressBar.rectTransform.sizeDelta = new Vector2(-offset, progressBar.rectTransform.sizeDelta.y);
        }
    }
}