using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sankusa.unity1week202309.InGame.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyCharacterController : EnemyComponentBase
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
