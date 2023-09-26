using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sankusa.unity1week202309.InGame.Stages
{
    public class Stage : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _validArea;
        public Bounds ValidArea => _validArea.bounds;
    }
}