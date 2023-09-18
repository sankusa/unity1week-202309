using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SankusaLib.ProfileLib {
    public class FpsText : MonoBehaviour
    {
        [SerializeField] private Text fpsText;
        [SerializeField] private float fpsCalcurateInterval = 0.5f;
        private int frameCount = 0;
        private float beforeTime = 0f;

        void Update() {
            frameCount++;
            float time = Time.realtimeSinceStartup - beforeTime;

            if(time >= fpsCalcurateInterval) {
                fpsText.text = (frameCount / time).ToString();

                frameCount = 0;
                beforeTime = Time.realtimeSinceStartup;
            }
        }
    }
}