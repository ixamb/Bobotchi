using Core.Services.Views;
using Core.UI.Components;
using TMPro;
using UnityEngine;

namespace Core.UI.ViewModels
{
    public sealed class AlertView : View
    {
        [SerializeField] private TMP_Text titleField;
        [SerializeField] private TMP_Text messageField;

        [SerializeField] private LabeledButton labeledButton;

        public void Initialize(AlertViewDto alertViewDto)
        {
            titleField.text = alertViewDto.AlertTitle;
            messageField.text = alertViewDto.AlertMessage;
            labeledButton.Initialize(alertViewDto.CloseButtonText, HideView, replaceListeners: true);
        }
    }

    public sealed record AlertViewDto
    {
        public string AlertTitle { get; set; }
        public string AlertMessage { get; set; }
        public string CloseButtonText { get; set; }
    }
}