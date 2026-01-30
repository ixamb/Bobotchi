using System;
using Core.Services.Views;
using Core.UI.Components;
using TMPro;
using UnityEngine;

namespace Core.UI.ViewModels
{
    public sealed class ModalView : View
    {
        [SerializeField] private TMP_Text titleField;
        [SerializeField] private TMP_Text messageField;
        [Space]
        [SerializeField] private LabeledButton validateButton;
        [SerializeField] private LabeledButton cancelButton;

        public void Initialize(ModalViewDto modalViewDto)
        {
            titleField.text = modalViewDto.ModalTitle;
            messageField.text = modalViewDto.ModalMessage;

            validateButton.Initialize(modalViewDto.ValidateButtonText, modalViewDto.ValidateButtonAction, replaceListeners: true);
            cancelButton.Initialize(modalViewDto.CancelButtonText, modalViewDto.CancelButtonAction, replaceListeners: true);
        }
    }
    
    public sealed record ModalViewDto
    {
        public string ModalTitle { get; set; }
        public string ModalMessage { get; set; }
        
        public Action ValidateButtonAction { get; set; }
        public string ValidateButtonText { get; set; }
        
        public Action CancelButtonAction { get; set; }
        public string CancelButtonText { get; set; }
    }
}