using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using SankusaLib;

namespace Sankusa.unity1week202309.InGame.Food
{
    [RequireComponent(typeof(Collider2D))]
    public class FoodBase : MonoBehaviour
    {
        [SerializeField] private int _energy;
        public virtual int Energy => _energy;

        [Inject] private FoodProvider _foodProvider;

        private Collider2D col;

        protected virtual void Awake()
        {
            _foodProvider.Add(this);
            col = GetComponent<Collider2D>();
            col.enabled = false;
        }

        void Start()
        {
            this.StartDelayCoroutine(0.3f, () => col.enabled = true);
        }

        public virtual void Eat()
        {
            Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            _foodProvider.Remove(this);
        }
    }
}