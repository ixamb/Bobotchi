using UnityEngine;

namespace BB.Services.Modules.PlayerPreferences.Handlers
{
    public abstract class PreferenceHandler
    {
        public abstract void Verify();
        
        protected static bool TryInitializePlayerPref<T>(string key, T value)
        {
            if (PlayerPrefs.HasKey(key))
                return false;

            if (value is int i)
            {
                PlayerPrefs.SetInt(key, i);
                return true;
            }

            if (value is float f)
            {
                PlayerPrefs.SetFloat(key, f);
                return true;
            }

            if (value is string s)
            {
                PlayerPrefs.SetString(key, s);
                return true;
            }
            
            return false;
        }
    }
}