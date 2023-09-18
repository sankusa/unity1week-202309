using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace SankusaLib.SoundLib {
    [RequireComponent(typeof(Slider))]
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private VolumeKey targetVolume;
        [SerializeField] private UnityEvent<float> onSliderValueChangedWithoutFirstTime = new UnityEvent<float>();
        private Slider slider;
        void Start() {
            slider = GetComponent<Slider>();
            UpdateSliderValue();
            slider.onValueChanged.AddListener(onSliderValueChangedWithoutFirstTime.Invoke);
            onSliderValueChangedWithoutFirstTime.AddListener(SliderValueChanged);
        }

        void Update() {
            UpdateSliderValue();
        }

        private void UpdateSliderValue() {
            slider.value = SoundManager.Instance.FindVolume(targetVolume.ToString()).Value * slider.maxValue;
        }

        public void SliderValueChanged(float value) {
            SoundManager.Instance.FindVolume(targetVolume.ToString()).Value = value / slider.maxValue;
        }
    }
}