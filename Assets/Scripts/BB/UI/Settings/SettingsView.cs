using BB.Actions;
using BB.Services.Modules.PlayerPreferences;
using BB.UI.Settings.Components;
using Core.Runtime.Extensions;
using Core.Runtime.Services.Audio;
using Core.Runtime.Services.Views;
using Core.Runtime.UI.Components.SingleSelectable;
using Core.Runtime.UI.ViewModels;
using UnityEngine;
using UnityEngine.UI;
using AudioType = Core.Runtime.Services.Audio.AudioType;

namespace BB.UI.Settings
{
    public sealed class SettingsView : View
    {
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [Space]
        [SerializeField] private BackgroundColorPickerComponent backgroundColorPickerPrefab;
        [SerializeField] private SingleSelectableGroup backgroundColorPickerGroup;
        [Space]
        [SerializeField] private Button deleteButton;

        private void Start()
        {
            InitializeColorPicker();
            InitializeAudioOptions();
            deleteButton.onClick.ReplaceListeners(RequestDeleteProgression);
        }

        private void InitializeColorPicker()
        {
            foreach (var color in PlayerPreferenceService.Instance.BackgroundColor.AvailableColors)
            {
                Instantiate(backgroundColorPickerPrefab, backgroundColorPickerGroup.GroupContainer)
                    .Initialize(new BackgroundColorPickerDto
                    {
                        Color = color,
                        IsActive = PlayerPreferenceService.Instance.BackgroundColor.ActiveBackgroundColor() == color,
                        OnClick = () => PlayerPreferenceService.Instance.BackgroundColor.Change(color),
                    });
            }
            backgroundColorPickerGroup.Initialize();
        }

        private void InitializeAudioOptions()
        {
            InitializeSlider(musicSlider, AudioType.Music);
            InitializeSlider(sfxSlider, AudioType.Sfx);
            return;

            void InitializeSlider(Slider slider, AudioType audioType)
            {
                slider.value = PlayerPreferenceService.Instance.Volume.GetVolume(audioType);
                slider.onValueChanged.AddListener(newVolume =>
                {
                    AudioService.Instance.ChangeVolume(audioType, newVolume);
                    PlayerPreferenceService.Instance.Volume.SetVolume(newVolume, audioType);
                });
            }
        }

        private static void RequestDeleteProgression()
        {
            var modalView = ViewService.Instance.GetView<ModalView>("modal-view");
            modalView.Initialize(new ModalViewDto
                {
                    ModalTitle = "Supprimer la progression",
                    ModalMessage = "Vous êtes sur le point de supprimer votre progression, êtes-vous sûr·e ?",
                    
                    ValidateButtonText = "Supprimer",
                    CancelButtonText = "Annuler",
                    
                    ValidateButtonAction = () =>
                    {
                        ScriptableObject.CreateInstance<DeleteProgressionAction>().Execute();
                    },
                    CancelButtonAction = () =>
                    {
                        modalView.HideView();
                    }
                });
            modalView.ShowView();
        }
    }
}