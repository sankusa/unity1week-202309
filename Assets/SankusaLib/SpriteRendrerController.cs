using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib {
    [ExecuteAlways()]
    public class SpriteRendrerController : MonoBehaviour
    {
        [SerializeField] private List<SpriteRenderer> renderers = new List<SpriteRenderer>();
        [SerializeField] private Color color = Color.white;

        void Update()
        {
            foreach(var renderer in renderers) {
                if(renderer != null && renderer.color != color) renderer.color = color;
            }
        }
    }
}