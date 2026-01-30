using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BB.Data;
using UnityEngine;

namespace BB.Services.Modules.PlayerPreferences.Handlers
{
    public sealed class BackgroundColorHandler : PreferenceHandler
    {
        private readonly List<IBackgroundColorHandlerObserver> _observers = new();

        public void RegisterObserver(IBackgroundColorHandlerObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void UnregisterObserver(IBackgroundColorHandlerObserver observer)
        {
            if (_observers.Contains(observer))
                _observers.Remove(observer);
        }
        
        private readonly List<Color> _availableColors = new()
        {
            FromHex("F7C8B6"),
            FromHex("A8E6CF"),
            FromHex("D4F1F4"),
            FromHex("E1CCEC"),
            FromHex("FFF2CC"),
            FromHex("FFD1DC"),
            FromHex("F0E5D8"),
            FromHex("B2D8D8"),
        };
        
        public override void Verify()
        {
            var defaultColor = _availableColors[0];
            if (TryInitializePlayerPref(Constants.PlayerPrefKeyParameters.BackgroundColor, ToRGBHex(defaultColor)))
                Change(defaultColor);
        }

        public void Change(Color color)
        {
            PlayerPrefs.SetString(Constants.PlayerPrefKeyParameters.BackgroundColor, ToRGBHex(color));
            _observers.ForEach(x => x.OnBackgroundColorChanged(color));
        }
        
        public Color ActiveBackgroundColor()
            => FromHex(PlayerPrefs.GetString(Constants.PlayerPrefKeyParameters.BackgroundColor));
        
        public IEnumerable<Color> AvailableColors => _availableColors;

        // from: https://discussions.unity.com/t/how-can-i-use-hex-color/193712/4
        private static Color FromHex(string hex)
        {
            if (hex.Length < 6)
            {
                throw new System.FormatException("Needs a string with a length of at least 6");
            }
            
            if (hex.First().Equals('#'))
                hex = hex[1..];

            var r = hex[..2];
            var g = hex.Substring(2, 2);
            var b = hex.Substring(4, 2);
            var alpha = hex.Length >= 8 ? hex.Substring(6, 2) : "FF";

            return new Color(
                r: int.Parse(r, NumberStyles.HexNumber) / 255f, 
                g: int.Parse(g, NumberStyles.HexNumber) / 255f, 
                b: int.Parse(b, NumberStyles.HexNumber) / 255f, 
                a: int.Parse(alpha, NumberStyles.HexNumber) / 255f);
        }

        private static string ToRGBHex(Color c)
        {
            return $"#{ToByte(c.r):X2}{ToByte(c.g):X2}{ToByte(c.b):X2}";
            
            byte ToByte(float f)
            {
                f = Mathf.Clamp01(f);
                return (byte)(f * 255);
            }
        }
    }

    public interface IBackgroundColorHandlerObserver
    {
        void OnBackgroundColorChanged(Color color);
    }
}