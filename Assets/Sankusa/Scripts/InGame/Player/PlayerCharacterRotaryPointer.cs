using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sankusa.unity1week202309.Common;

namespace Sankusa.unity1week202309.InGame.Player
{
    public class PlayerCharacterRotaryPointer : PlayerCharacterComponentBase
    {
        [SerializeField] private RotaryPointer _rotaryPointer;
        public RotaryPointer RotaryPointer => _rotaryPointer;
    }
}