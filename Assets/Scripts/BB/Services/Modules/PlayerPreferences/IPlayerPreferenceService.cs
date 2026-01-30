using BB.Services.Modules.PlayerPreferences.Handlers;
using Core.Services;

namespace BB.Services.Modules.PlayerPreferences
{
    public interface IPlayerPreferenceService : ISingleton
    {
        public BackgroundColorHandler BackgroundColor { get; }
        public VolumePreferenceHandler Volume { get; }
    }
}