using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sankusa.unity1week202309.InGame.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerCharacterController : PlayerCharacterComponentBase
    {
        private Rigidbody2D _rigidBody;
        public Vector2 Velocity
        {
            get => _rigidBody.velocity;
            set => _rigidBody.velocity = value;
        }

        void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }
    }
}