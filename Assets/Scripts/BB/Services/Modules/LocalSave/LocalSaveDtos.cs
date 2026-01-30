using System;
using System.Collections.Generic;
using System.Linq;
using BB.Data;
using BB.Management.FurniturePlacement;
using JetBrains.Annotations;

namespace BB.Services.Modules.LocalSave
{
    public sealed record PlayInformationSaveDto : SaveDto
    {
        public DateTime LastClosed { get; set; } = DateTime.Now;
    }

    public sealed record PlayerInformationSaveDto : SaveDto
    {
        [CanBeNull] public string Surname { get; set; }
        public BodyType? BodyType { get; set; }
        public Gender? Gender { get; set; }

        public bool IsInitialized() => Surname == null || BodyType == null || Gender == null;
    }
    
    public sealed record PlayerStatStateSaveDto : SaveDto
    {
        public List<PlayerStatStateEntrySaveDto> StateStatEntries { get; set; } = ((CharacterStateStat[]) Enum.GetValues(typeof(CharacterStateStat)))
            .Select(stat => new PlayerStatStateEntrySaveDto
            {
                CharacterStat = stat,
                Amount = Constants.ProgressStatStateThresholds.Maximum,
            }).ToList();
            
        public sealed record PlayerStatStateEntrySaveDto
        {
            public CharacterStateStat CharacterStat { get; set; }
            public float Amount { get; set; }
        }
    }

    public sealed record BalanceSaveDto : SaveDto
    {
        public List<BalanceEntrySaveDto> Balances { get; set; } = ((Currency[]) Enum.GetValues(typeof(Currency)))
            .Select(currency => new BalanceEntrySaveDto
            {
                Currency = currency,
                Amount = currency switch
                {
                    Currency.Common => 500f,
                    _ => 0f,
                }
            }).ToList();

        public sealed record BalanceEntrySaveDto
        {
            public Currency Currency { get; set; }
            public float Amount { get; set; }
        }
    }

    public sealed record InventorySaveDto : SaveDto
    {
        public List<InventoryEntrySaveDto> PurchasableEntitiesInInventory { get; set; } = new();
        public sealed record InventoryEntrySaveDto
        {
            public Guid EntityGuid { get; set; }
            public PurchasableEntityType PurchasableEntityType { get; set; }
            public uint Quantity { get; set; }
        }
    }

    public sealed record FurniturePlacementSaveDto : SaveDto
    {
        public Guid FloorGuid { get; set; }
        public Guid WallGuid { get; set; }
        public List<PropPlacementEntrySaveDto> PropPlaced { get; set; } = new();
        
        public sealed record PropPlacementEntrySaveDto
        {
            public Guid PlacementEntryGuid { get; set; }
            public Guid PropGuid { get; set; }
            public int GridX { get; set; }
            public int GridY { get; set; }
            public PropPlacementDirection Direction { get; set; }
        }
    }

    public sealed record RunningMissionDto : SaveDto
    {
        public Guid? MissionGuid { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
    
    public abstract record SaveDto;
}