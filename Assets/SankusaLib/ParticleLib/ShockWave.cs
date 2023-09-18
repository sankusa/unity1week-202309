using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib.ParticleLib {
    [RequireComponent(typeof(ParticleSystem))]
    public class ShockWave : MonoBehaviour
    {
        private ParticleSystem ps;
        private float elapsedScaleUpTime = 0;
        [SerializeField] private float scaleUpTime = 0.03f;
        [SerializeField] private float scaleUpRate = 0.1f;
        private float elapsedDeleteTime = 0f;
        [SerializeField] private float deleteTime = 5f;

        void Awake() {
            ps = GetComponent<ParticleSystem>();
        }

        void Update()
        {
            elapsedScaleUpTime += Time.deltaTime;
            elapsedDeleteTime += Time.deltaTime;

            if(elapsedDeleteTime >= deleteTime) {
                Destroy(gameObject);
            }

            if(elapsedScaleUpTime > scaleUpTime) {
                transform.localScale += new Vector3(scaleUpRate, scaleUpRate, scaleUpRate);
                elapsedScaleUpTime = 0;
            }
        }
    }
}