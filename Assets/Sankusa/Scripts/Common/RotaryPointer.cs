using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Sankusa.unity1week202309.Common
{
    public class RotaryPointer : MonoBehaviour
    {
        private Transform _transformCache;
        // 回転/s
        [SerializeField, Min(0)] private float _rotateSpeed;
        private float _angle;

        [SerializeField] private bool _rotateRight = true;
        public bool RotateRight
        {
            get => _rotateRight;
            set => _rotateRight = value;
        }

        [SerializeField] private bool _active = true;
        public bool Active
        {
            get => _active;
            set => _active = value;
        }

        public Vector2 Direction
        {
            get
            {
                // float angle = _transformCache.localEulerAngles.z / 360 * 2 * Mathf.PI;
                // Debug.Log(_transformCache.rotation.eulerAngles.z + "/" + angle + "/" + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
                // return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                return transform.up;
            }
        }
        public float NormalizedValue => _transformCache.rotation.z / 360;
        
        void Start()
        {
            _transformCache = transform;
            _angle = _transformCache.localPosition.z;
        }

        void Update()
        {
            if(_active)
            {
                _transformCache.Rotate(0f, 0f, (_rotateRight ? -1 : 1) * _rotateSpeed * 360 * Time.deltaTime);
            }
        }
    }
}