using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SankusaLib {
    public class DualImage : MonoBehaviour
    {
        [SerializeField] private Image image1;
        [SerializeField] private Image image2;
        [SerializeField] private float period = 1f;
        
        void Update() {
            image1.color = new Color(image1.color.r, image1.color.g, image1.color.b, Mathf.Sin(2 * Mathf.PI * Time.time / period));
            image2.color = new Color(image2.color.r, image2.color.g, image2.color.b, 1 - Mathf.Sin(2 * Mathf.PI * Time.time / period));
        }
    }
}