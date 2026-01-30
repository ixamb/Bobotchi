using System.Linq;
using BB.Data;
using BB.Services.Modules.LocalSave;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.Common.Components
{
    public class StatBarSmallComponent : MonoBehaviour, BBSetLocalSaveObserver
    {
        [SerializeField] private CharacterStateStat stateStat;
        [Space]
        [SerializeField][Range(0f,1f)] private float minoredAlphaValue;
        [SerializeField][Range(0f,1f)] private float normalAlphaValue;
        [SerializeField] private Image smallBar;
        [SerializeField] private Image mediumBar;
        [SerializeField] private Image bigBar;

        private void Start()
        {
            BBLocalSaveService.Instance.RegisterSetLocalSaveObserver(this);
            UpdateBarComponent(BBLocalSaveService.Instance.StateStat.Get(stateStat));
        }
        
        
        public void OnLocalSaved<T>(string key, T value)
        {
            if (value is not PlayerStatStateSaveDto currencySaveDto)
                return;

            UpdateBarComponent(currencySaveDto.StateStatEntries.FirstOrDefault(entry => entry.CharacterStat == stateStat)?.Amount ?? 0);
        }
        
        private void UpdateBarComponent(float statValue)
        {
            ChangeAlphaColor(bigBar, statValue >= 75f ? normalAlphaValue : minoredAlphaValue);
            ChangeAlphaColor(mediumBar, statValue >= 50f ? normalAlphaValue : minoredAlphaValue);
            ChangeAlphaColor(smallBar, statValue >= 25f ? normalAlphaValue : minoredAlphaValue);
        }
        

        private static void ChangeAlphaColor(Image image, float alphaValue)
        {
            var color = image.color;
            image.color = new Color(color.r, color.g, color.b, alphaValue);
        }
    }
}