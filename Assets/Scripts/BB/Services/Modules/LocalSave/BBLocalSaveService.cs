using System.Collections.Generic;
using BB.Services.Modules.LocalSave.Handlers;
using Core.Runtime.Services.LocalSave;
using Core.Runtime.Services;

namespace BB.Services.Modules.LocalSave
{
    // ReSharper disable once InconsistentNaming
    public sealed class BBLocalSaveService : Singleton<BBLocalSaveService, IBBLocalSaveService>, IBBLocalSaveService
    {
        PlayInformationSaveHandler IBBLocalSaveService.PlayInformation => _playInformationSaveHandler;
        private PlayInformationSaveHandler _playInformationSaveHandler;
        
        PlayerInformationSaveHandler IBBLocalSaveService.PlayerInformation => _playerInformationSaveHandler;
        private PlayerInformationSaveHandler _playerInformationSaveHandler;
        
        BalanceSaveHandler IBBLocalSaveService.Balance => _balanceSaveHandler;
        private BalanceSaveHandler _balanceSaveHandler;
        
        PurchasableEntitySaveHandler IBBLocalSaveService.PurchasableEntities =>  _purchasableEntitySaveHandler;
        private PurchasableEntitySaveHandler _purchasableEntitySaveHandler;
        
        StateStatSaveHandler IBBLocalSaveService.StateStat => _stateStatSaveHandler;
        private StateStatSaveHandler _stateStatSaveHandler;
        
        FurniturePlacementSaveHandler IBBLocalSaveService.FurniturePlacement => furniturePlacementSaveHandler;
        private FurniturePlacementSaveHandler furniturePlacementSaveHandler;
        
        RunningMissionSaveHandler IBBLocalSaveService.RunningMission => _runningMissionSaveHandler;
        private RunningMissionSaveHandler _runningMissionSaveHandler;
        
        private readonly List<BBSetLocalSaveObserver> _setLocalSaveObservers = new();

        public void RegisterSetLocalSaveObserver(BBSetLocalSaveObserver observer)
        {
            if (_setLocalSaveObservers.Contains(observer))
                return;

            _setLocalSaveObservers.Add(observer);
        }

        public void UnregisterSetLocalSaveObserver(BBSetLocalSaveObserver observer)
        {
            if (!_setLocalSaveObservers.Contains(observer))
                return;

            _setLocalSaveObservers.Remove(observer);
        }
        
        protected override void Init()
        {
            InitializeSaveHandlers();
        }

        private void InitializeSaveHandlers()
        {
            _playInformationSaveHandler = new PlayInformationSaveHandler()
                .Bind(this).Verify() as PlayInformationSaveHandler;
            
            _playerInformationSaveHandler = new PlayerInformationSaveHandler()
                .Bind(this).Verify() as PlayerInformationSaveHandler;
            
            _balanceSaveHandler = new BalanceSaveHandler()
                .Bind(this).Verify() as BalanceSaveHandler;
            
            _purchasableEntitySaveHandler = new PurchasableEntitySaveHandler()
                .Bind(this).Verify() as PurchasableEntitySaveHandler;
            
            _stateStatSaveHandler = new StateStatSaveHandler()
                .Bind(this).Verify() as StateStatSaveHandler;
            
            furniturePlacementSaveHandler = new FurniturePlacementSaveHandler()
                .Bind(this).Verify() as FurniturePlacementSaveHandler;
            
            _runningMissionSaveHandler = new RunningMissionSaveHandler()
                .Bind(this).Verify() as RunningMissionSaveHandler;
        }
        
        public void Save()
        {
            LocalSaveService.Instance.Save();
        }

        public void Delete()
        {
            LocalSaveService.Instance.Delete();
            Init();
        }

        // ReSharper disable once MemberCanBeMadeStatic.Global
        internal T Get<T>(string key, T defaultValue = default)
        {
            return LocalSaveService.Instance.Get(key, defaultValue);
        }

        internal void Set<T>(string key, T value, bool autoSave = false)
        {
            LocalSaveService.Instance.Set(key, value, autoSave);
            foreach (var observer in _setLocalSaveObservers)
                observer.OnLocalSaved(key, value);
        }
    }
}