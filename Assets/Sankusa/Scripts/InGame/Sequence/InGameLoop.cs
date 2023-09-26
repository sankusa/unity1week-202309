using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using Sankusa.unity1week202309.InputManagement;
using Sankusa.unity1week202309.Scene;
using Sankusa.unity1week202309.InGame.GameStatus;
using Sankusa.unity1week202309.InGame.Player;
using Sankusa.unity1week202309.InGame.Performer;


namespace Sankusa.unity1week202309.InGame.Sequence
{
    public class InGameLoop : IInitializable, IDisposable
    {
        private readonly DayModel _dayModel;
        private readonly DayTimer _dayTimer;
        private readonly ScoreModel _scoreModel;
        private readonly IInputProvider _inputProvider;
        private readonly PlayerCharacterStatus _playerStatus;
        private readonly DayScenarioExcecuter _dayScenarioExcecuter;
        private readonly TimeUpPerformer _timeUpPerformer;
        private readonly DeadPerformer _deadPerformer;
        private readonly SurvivePerformer _survivePerformer;
        private readonly CancellationTokenSource source = new CancellationTokenSource();

        [Inject]
        public InGameLoop(
            DayModel dayModel,
            DayTimer dayTimer,
            ScoreModel scoremodel,
            IInputProvider inputProvider,
            PlayerCharacterStatus playerStatus,
            DayScenarioExcecuter dayScenarioExcecuter,
            TimeUpPerformer timeUpPerformer,
            DeadPerformer deadPerformer,
            SurvivePerformer survivePerformer
        )
        {
            _dayModel = dayModel;
            _dayTimer = dayTimer;
            _scoreModel = scoremodel;
            _inputProvider = inputProvider;
            _playerStatus = playerStatus;
            _dayScenarioExcecuter = dayScenarioExcecuter;
            _timeUpPerformer = timeUpPerformer;
            _deadPerformer = deadPerformer;
            _survivePerformer = survivePerformer;
        }

        public void Initialize()
        {
            StartAsync(source.Token).Forget();
        }

        private async UniTask StartAsync(CancellationToken token)
        {
            while(true)
            {
                // 日時開始処理
                _playerStatus.Reflesh();
                _dayModel.Increment();
                _dayTimer.Reset();
                _dayTimer.SetLimit(180);
                _dayTimer.Start();

                _dayScenarioExcecuter.Execute(_dayModel.Day.Value, token).Forget();

                while(true)
                {
                    if(_dayTimer.IsTimeOver)
                    {
                        if(_playerStatus.EnergyIsFull)
                        {
                            await OnTimeSurvive(token);
                        }
                        else
                        {
                            await OnTimeOver(token);
                        }
                        
                        break;
                    }

                    if(_playerStatus.Hp == 0)
                    {
                        await OnDead(token);
                        break;
                    }

                    await UniTask.Yield(token);
                }

                // 日時終了処理
                _dayTimer.Stop();

                // ゲーム終了
                naichilab.RankingLoader.Instance.SendScoreAndShowRanking(_scoreModel.Score.Value);
                // ランキング表示前にシーン遷移するのを防ぐ
                await UniTask.Delay(1000, cancellationToken: token);
                while(true)
                {
                    await UniTask.WaitUntil(() => _inputProvider.GetMainButtonDown());
                    // ランキング消えたか判定
                    if(GameObject.FindObjectOfType<naichilab.RankingSceneManager>() == null)
                    {
                        break;
                    }
                }
                
                SceneLoader.LoadTitleScene();

                // 次の日には入らない(仕様変更により日付の概念は削除)
                break;

                await UniTask.Yield(token);
            }
        }

        private async UniTask OnTimeSurvive(CancellationToken token)
        {
            await _survivePerformer.Perform(token);
            await UniTask.Delay(1000, cancellationToken: token);
        }

        private async UniTask OnTimeOver(CancellationToken token)
        {
            await _timeUpPerformer.Perform(token);
            await UniTask.Delay(1000, cancellationToken: token);
        }

        private async UniTask OnDead(CancellationToken token)
        {
            await _deadPerformer.Perform(token);
            await UniTask.Delay(1000, cancellationToken: token);
        }

        public void Dispose()
        {
            source.Cancel();
        }
    }
}