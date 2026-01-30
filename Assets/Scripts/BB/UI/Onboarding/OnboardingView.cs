using System;
using System.Linq;
using BB.Data;
using BB.Services.Modules.GameData;
using BB.Services.Modules.LocalSave;
using Core.Extensions;
using Core.Services.Views;
using Core.UI.Components.SingleSelectable;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BB.UI.Onboarding
{
    public class OnboardingView : View
    {
        [SerializeField] private TMP_InputField nameField;
        [SerializeField] private SingleSelectableGroup bodyTypeGroup;
        [SerializeField] private SingleSelectableGroup genderGroup;
        [SerializeField] private TMP_Text errorText;
        [SerializeField] private Button validateButton;

        private void Start()
        {
            validateButton.onClick.ReplaceListeners(async () =>
            {
                var inputName = nameField.text;
                if (string.IsNullOrWhiteSpace(inputName))
                {
                    InitializeErrorField("Un nom est nécessaire !");
                    return;
                }

                var bodyType = bodyTypeGroup.SelectedElement?.GetComponent<BodyTypeComponent>().BodyType;
                if (bodyType is null)
                {
                    InitializeErrorField("Un type de corps doit être sélectionné !");
                    return;
                }
                
                var gender = genderGroup.SelectedElement?.GetComponent<GenderComponent>().Gender;
                if (gender is null)
                {
                    InitializeErrorField("Un genre doit être sélectionné !");
                    return;
                }
                
                InitializePlayerInformation(inputName, bodyType!.Value, gender!.Value);
                InitializeInventoryData();
                BBLocalSaveService.Instance.Save();
                
                await Core.Services.Scenes.SceneService.Instance.LoadSceneAsync(Constants.SceneNames.Game,
                    LoadSceneMode.Single);
            });
        }

        private void InitializeErrorField(string error)
        {
            errorText.text = error;
            errorText.gameObject.SetActive(true);
        }

        private void InitializePlayerInformation(string surname, BodyType bodyType, Gender gender)
        {
            BBLocalSaveService.Instance.PlayerInformation.Register(new PlayerInformationSaveDto
            {
                Surname = surname,
                BodyType = bodyType,
                Gender = gender,
            });
        }

        private void InitializeInventoryData()
        {
            var defaultFloor = GameDataService.Instance.GetFloors().First(floor => floor.IsDefault);
            BBLocalSaveService.Instance.PurchasableEntities.Update(PurchasableEntityType.Furniture, defaultFloor, UpdateOperation.Add, 1);
            BBLocalSaveService.Instance.FurniturePlacement.Place(defaultFloor);
            BBLocalSaveService.Instance.PurchasableEntities.Update(PurchasableEntityType.Furniture, defaultFloor, UpdateOperation.Remove, 1);
            
            var defaultWall = GameDataService.Instance.GetWalls().First(wall => wall.IsDefault);
            BBLocalSaveService.Instance.PurchasableEntities.Update(PurchasableEntityType.Furniture, defaultWall, UpdateOperation.Add, 1);
            BBLocalSaveService.Instance.FurniturePlacement.Place(defaultWall);
            BBLocalSaveService.Instance.PurchasableEntities.Update(PurchasableEntityType.Furniture, defaultWall, UpdateOperation.Remove, 1);
            
            BBLocalSaveService.Instance.Save();
        }
    }
}