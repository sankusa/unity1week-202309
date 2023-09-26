using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sankusa.unity1week202309.InGame.Food
{
    public class FoodProvider
    {
        private HashSet<FoodBase> _foods = new HashSet<FoodBase>();
        public IEnumerable<FoodBase> Foods => _foods;

        public void Add(FoodBase food)
        {
            _foods.Add(food);
        }

        public void Remove(FoodBase food)
        {
            _foods.Remove(food);
        }
    }
}