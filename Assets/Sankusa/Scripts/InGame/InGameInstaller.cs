using UnityEngine;
using Zenject;
using Sankusa.unity1week202309.InGame.GameStatus;
using Sankusa.unity1week202309.InGame.Player;
using Sankusa.unity1week202309.InGame.Enemy;
using Sankusa.unity1week202309.InGame.Food;
using Sankusa.unity1week202309.InGame.Sequence;
using Sankusa.unity1week202309.InGame.Stages;
using Sankusa.unity1week202309.InGame.Performer;

namespace Sankusa.unity1week202309.InGame
{
    public class InGameInstaller : MonoInstaller
    {
        [SerializeField] private PlayerCharacterDefaultStatusPreset _statusPreset;
        [SerializeField] private EnemyMaster _enemyMaster;
        [SerializeField] private Stage _stage;
        [SerializeField] private TimeUpPerformer _timeUpPerformer;
        [SerializeField] private DeadPerformer _deadPerformer;
        [SerializeField] private SurvivePerformer _survivePerformer;

        public override void InstallBindings()
        {
            // GameStatus
            Container
                .BindInterfacesAndSelfTo<DayModel>()
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<DayTimer>()
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<ScoreModel>()
                .AsSingle()
                .NonLazy();

            // Food
            Container
                .BindInterfacesAndSelfTo<FoodProvider>()
                .AsSingle()
                .NonLazy();

            // Player
            Container
                .BindInterfacesAndSelfTo<PlayerCharacterProvider>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<PlayerCharacterDefaultStatusPreset>()
                .FromInstance(_statusPreset)
                .AsCached();

            Container
                .BindInterfacesAndSelfTo<PlayerCharacterStatus>()
                .AsSingle()
                .NonLazy();

            // Enemy
            Container
                .Bind<EnemyMaster>()
                .FromInstance(_enemyMaster)
                .AsCached();

            Container
                .BindInterfacesAndSelfTo<EnemyProvider>()
                .AsSingle()
                .NonLazy();

            // GameStatus(特例)
            Container
                .BindInterfacesAndSelfTo<ScoreCalculator>()
                .AsSingle()
                .NonLazy();

            // Stage
            Container
                .Bind<Stage>()
                .FromInstance(_stage)
                .AsCached();

            // Performer
            Container
                .Bind<TimeUpPerformer>()
                .FromInstance(_timeUpPerformer)
                .AsCached();

            Container
                .Bind<DeadPerformer>()
                .FromInstance(_deadPerformer)
                .AsCached();

            Container
                .Bind<SurvivePerformer>()
                .FromInstance(_survivePerformer)
                .AsCached();

            // Sequence
            Container
                .BindInterfacesAndSelfTo<DayScenarioExcecuter>()
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<InGameLoop>()
                .AsSingle()
                .NonLazy();
        }
    }
}