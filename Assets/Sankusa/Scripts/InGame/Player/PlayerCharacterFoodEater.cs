using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sankusa.unity1week202309.InGame.Food;
using UniRx.Triggers;
using SankusaLib.SoundLib;

namespace Sankusa.unity1week202309.InGame.Player
{
    public class PlayerCharacterFoodEater : PlayerCharacterComponentBase
    {
        [SerializeField, SoundId] private string eatSeId;
        protected override void OnInitialize()
        {
            this.OnTriggerEnter2DAsObservable()
                .Subscribe(col =>
                {
                    FoodBase food = col.gameObject.GetComponent<FoodBase>();
                    if(food != null)
                    {
                        _core.Status.AddEnergy(food.Energy);
                        food.Eat();
                        SoundManager.Instance.PlaySe(eatSeId);
                    }
                })
                .AddTo(this);
        }
    }
}