using System;
using BB.Grid;
using TheForge.Services.Scheduler;
using UnityEngine;

namespace BB
{
    public sealed class ActorController : MonoBehaviour
    {
        [SerializeField] private Actor.Actor actorPrefab;
        
        private Actor.Actor _spawnedActor;
        private Grid.Tiles.Tile _currentTile;
        
        private void Start()
        {
            Spawn();
        }

        private void Spawn()
        {
            _currentTile = GridManager.Instance.PickRandomFreeTile();
            if (_currentTile is null)
                throw new Exception("An error occured while trying to spawn an actor: no free tile could be found.");
            
            _spawnedActor = Instantiate(actorPrefab, _currentTile.transform.position, _currentTile.transform.rotation);
            _spawnedActor.transform.SetParent(transform);
            InitializeRandomMoveActionScheduler();
        }

        private void InitializeRandomMoveActionScheduler()
        {
            ActionSchedulerService.Instance.CreateScheduler(
                code: "move",
                action: () =>
                {
                    _currentTile = GridManager.Instance
                        .PickRandomFreeTileCloseToTile(tile: _currentTile, _spawnedActor.MovementProperties.RandomMovePerimeter);
                    if (_currentTile is null)
                        return;
                    
                    _spawnedActor.MoveTo(_currentTile.GetCharacterAnchor.position);
                },
                durationInSeconds: _spawnedActor.MovementProperties.RandomMoveIntervalInSeconds,
                endAction: SchedulerEndAction.Repeat);
        }

        private void EndRandomMoveActionScheduler()
        {
            ActionSchedulerService.Instance.DestroyScheduler("move");
        }
    }
}