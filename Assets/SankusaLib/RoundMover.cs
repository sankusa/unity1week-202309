using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SankusaLib {
    public class RoundMover : MonoBehaviour
    {
        [SerializeField, Min(0)] private float radiusX;
        [SerializeField, Min(0)] private float periodX = 1f;
        [SerializeField] private float phaseAngleX;
        [SerializeField, Min(0)] private float radiusY;
        [SerializeField, Min(0)] private float periodY = 1f;
        [SerializeField] private float phaseAngleY;
        private float radianX;
        private float centerX;
        private float radianY;
        private float centerY;

        void Awake() {
            radianX = 2f * Mathf.PI * phaseAngleX / 360f;
            centerX = transform.localPosition.x - radiusX * Mathf.Cos(radianX);
            radianY = 2f * Mathf.PI * phaseAngleY / 360f;
            centerY = transform.localPosition.y - radiusY * Mathf.Cos(radianY);
        }

        void Update() {
            radianX += 2f * Mathf.PI * Time.deltaTime / periodX;
            radianY += 2f * Mathf.PI * Time.deltaTime / periodY;

            Vector2 position = transform.localPosition;
            position.x = centerX + radiusX * Mathf.Cos(radianX);
            position.y = centerY + radiusY * Mathf.Cos(radianY);
            transform.localPosition = position;
        }
    }
}