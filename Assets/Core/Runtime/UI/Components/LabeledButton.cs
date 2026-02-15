using System;
using Core.Runtime.Extensions;
using TMPro;
using UnityEngine.UI;

namespace Core.Runtime.UI.Components
{
    public class LabeledButton : Button
    {
        private TMP_Text _label;
        protected override void Awake()
        {
            base.Awake();
            _label = GetComponentInChildren<TMP_Text>();
        }

        public void Initialize(string label, Action onButtonClick, bool replaceListeners = false)
        {
            _label.text = label;
            if (replaceListeners)
                onClick.ReplaceListeners(() => onButtonClick?.Invoke());
            else
                onClick.AddListener(() => onButtonClick?.Invoke());
        }

        public void Initialize<T>(string label, Action<T> onButtonClick, T value, bool replaceListeners = false)
        {
            _label.text = label;
            if (replaceListeners)
                onClick.ReplaceListeners(() => onButtonClick(value));
            else
                onClick.AddListener(() => onButtonClick(value));
        }
    }
}