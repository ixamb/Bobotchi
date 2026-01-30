using BB.Services.Modules.PlayerPreferences.Handlers;
using Core.Services;

namespace BB.Services.Modules.PlayerPreferences
{
    public sealed class PlayerPreferenceService : Singleton<PlayerPreferenceService, IPlayerPreferenceService>, IPlayerPreferenceService
    {
        private BackgroundColorHandler _backgroundColorHandler;
        private VolumePreferenceHandler _volumeHandler;
        
        protected override void Init()
        {
            _backgroundColorHandler = new BackgroundColorHandler();
            _backgroundColorHandler.Verify();
            
            _volumeHandler = new VolumePreferenceHandler();
            _volumeHandler.Verify();
        }
        
        BackgroundColorHandler IPlayerPreferenceService.BackgroundColor => _backgroundColorHandler;
        VolumePreferenceHandler IPlayerPreferenceService.Volume => _volumeHandler;
    }
}