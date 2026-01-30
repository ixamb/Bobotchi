using BB.Data;
using BB.Services.Modules.LocalSave;
using Core.Systems.Actions;
using UnityEngine;

namespace BB.Actions
{
    [CreateAssetMenu(fileName = "State Stat Effect Action", menuName = "BB/Actions/State Stat Effect Action")]
    public class StateStatEffectAction : GameAction
    {
        [SerializeField] private CharacterStateStat affectedState;

        [SerializeField] [Range(-Constants.ProgressStatStateThresholds.Maximum, Constants.ProgressStatStateThresholds.Maximum)]
        private float affection;
        
        protected override void Executable()
        {
            BBLocalSaveService.Instance.StateStat.Update(affectedState, affection);
        }
        
        public CharacterStateStat AffectedState => affectedState;
        public float Affection => affection;
    }
}