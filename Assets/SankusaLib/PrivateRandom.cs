using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib
{
    public class PrivateRandom
    {
        private Random.State state;

        public PrivateRandom() : this((int) System.DateTime.Now.Ticks){}

        public PrivateRandom(int seed)
        {
            Random.State tmpState = Random.state;
            Random.InitState(seed);
            state = Random.state;
            Random.state = tmpState;
        }

        public float NextValue
        {
            get {
                Random.State tmpState = Random.state;
                Random.state = state;
                float result = Random.value;
                state = Random.state;
                Random.state = tmpState;
                return result;
            }
        }
    }
}