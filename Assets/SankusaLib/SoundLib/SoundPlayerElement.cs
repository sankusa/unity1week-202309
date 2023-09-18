using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.Events;

namespace SankusaLib.SoundLib {
    // 注)フェード遅延中に新たにフェード遅延をしようとするとフェード遅延情報が上書きされる
    public class SoundPlayerElement
    {
        private AudioSource audioSource;
        private SoundData soundData;
        private List<Volume> volumeList;
        private Volume fadeVolume;
        private PlayLog playLog;
        // フェード用
        private const string VOLUME_KEY_FADE = "VolumeKeyFade";
        private bool isFading = false;
        private float fadeDuration = 0;
        private float fadeVolumeFrom = 0;
        private float fadeVolumeTo = 0;
        private float fadeTimer = 0;

        public bool IsPlaying => audioSource.isPlaying;

        public string SoundId => soundData != null ? soundData.Id : "";

        private UnityEvent onFadeFinish = new UnityEvent();

        public SoundPlayerElement(GameObject target, List<Volume> volumeList, PlayLog playLog) {
            audioSource = target.AddComponent<AudioSource>();
            this.volumeList = new List<Volume>();
            foreach(Volume volume in volumeList) {
                this.volumeList.Add(volume);
            }
            // フェード用Volume
            fadeVolume = new Volume(new VolumeSetting(VOLUME_KEY_FADE, false, 1, false), "");
            this.volumeList.Add(fadeVolume);
            foreach(Volume vol in volumeList) {
                vol.OnValueChanged.Subscribe(_ => UpdateVolume());
                vol.OnMuteChanged.Subscribe(_ => UpdateMute());
            }
            this.playLog = playLog;
        }

        public void Update() {
            UpdateVolume();

            // フェード
            if(isFading) {
                fadeTimer += Time.deltaTime;
                if(fadeDuration == 0) {
                    volumeList.Find(x => x.Key == VOLUME_KEY_FADE).Value = fadeVolumeTo;
                } else {
                    volumeList.Find(x => x.Key == VOLUME_KEY_FADE).Value = Mathf.Lerp(fadeVolumeFrom, fadeVolumeTo, fadeTimer / fadeDuration);
                }
                if(fadeTimer >= fadeDuration) {
                    fadeVolume.Value = 1;
                    isFading = false;
                    fadeDuration = 0;
                    fadeVolumeFrom = 0;
                    fadeVolumeTo = 0;
                    fadeTimer = 0;
                    onFadeFinish?.Invoke();
                    onFadeFinish.RemoveAllListeners();
                }
            }

            if(soundData != null && soundData.Clip != null && audioSource.time >= soundData.End * soundData.Clip.length) {
                if(audioSource.loop) {
                    audioSource.time = soundData.Start * soundData.Clip.length;
                } else {
                    Stop();
                }
            }
        }

        private void UpdateVolume() {
            float volume = 1f;
            foreach(Volume vol in volumeList) {
                volume *= vol.Value;
            }
            if(soundData != null) {
                if(soundData.VolumeType == VolumeType.Constant) {
                    volume *= soundData.Volume;
                } else if(soundData.VolumeType == VolumeType.Curve) {
                    volume *= Mathf.Clamp01(soundData.VolumeCurve.Evaluate(audioSource.time));
                }
                
            }
            audioSource.volume = volume;
        }

        private void UpdateMute() {
            bool mute = false;
            foreach(Volume vol in volumeList) {
                mute |= vol.Mute;
            }
            audioSource.mute = mute;
        }

        private void Reset() {
            soundData = null;

            fadeVolume.Value = 1;
            isFading = false;
            fadeDuration = 0;
            fadeVolumeFrom = 0;
            fadeVolumeTo = 0;
            fadeTimer = 0;
            onFadeFinish.RemoveAllListeners();
        }

        public void Play(string soundId, bool loop) {

            Reset();

            // 特殊ID(-n:n個前に再生したidで再生)
            if(int.TryParse(soundId, out int num) && num < 0) {
                // 履歴が足りなければ再生しない
                if(playLog.Records.Count < Mathf.Abs(num)) {
                    playLog.Clear();
                    return;
                } else {
                    PlayLogRecord target = playLog.Records[playLog.Records.Count - Mathf.Abs(num)];
                    playLog.DeleteNewRecord(Mathf.Abs(num));
                    Play(target.SoundId, target.Loop);
                    return;
                }
            }

            soundData = SoundDataMaster.Instance.FindSoundData(soundId);
            if(soundData == null) {
                Debug.LogWarning("SoundData is null. SoundId = " + soundId);
                return;
            }
            if(soundData.Clip == null) {
                Debug.LogWarning("AudioClip is null. SoundId = " + soundId);
                return;
            }
            audioSource.clip = soundData.Clip;
            audioSource.pitch = soundData.Pitch;
            audioSource.loop = loop;
            audioSource.time = soundData.Start * soundData.Clip.length;

            UpdateVolume();
            audioSource.Play();

            playLog?.Add(new PlayLogRecord(soundId, loop));
        }

        public void Stop() {
            audioSource.Stop();
            Reset();
        }

        public void Fade(float duration, float from, float to, UnityAction onFadeFinish = null) {
            fadeVolume.Value = from;
            isFading = true;
            fadeTimer = 0;
            fadeDuration = duration;
            fadeVolumeFrom = from;
            fadeVolumeTo = to;
            if(onFadeFinish != null) {
                this.onFadeFinish.AddListener(onFadeFinish);
            }
        }
    }
}