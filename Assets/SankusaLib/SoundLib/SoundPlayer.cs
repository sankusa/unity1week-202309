using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;
using UniRx;

namespace SankusaLib.SoundLib {
    public class SoundPlayer
    {
        private string key;
        public string Key => key;

        private bool defaultLoop;
        public bool DefaultLoop => defaultLoop;

        private float defaultFadeDuration;
        private float DefaultFadeDuration => defaultFadeDuration;

        private List<SoundPlayerElement> elements = new List<SoundPlayerElement>();
        public List<SoundPlayerElement> Elements => elements;

        private PlayLog playLog;

        public SoundPlayer(GameObject root, SoundPlayerSetting setting, List<Volume> allVolumeList) {
            key = setting.Key;
            defaultLoop = setting.DefaultLoop;
            defaultFadeDuration = setting.DefaultFadeDuration;
            // AudioSource用GameObject作成
            GameObject go = new GameObject(setting.Key);
            go.transform.parent = root.transform;
            // 使用するVolumeのリストを作成
            List<Volume> volumeList = new List<Volume>();
            foreach(string volumeKey in setting.VolumeKeys) {
                Volume vol = allVolumeList.Find(x => x.Key == volumeKey);
                if(vol != null) volumeList.Add(vol);
            }
            // プレイヤー作成
            playLog = new PlayLog(setting.LogCapacity);
            for (int i = 0; i < setting.PlayerCount; i++) {
                elements.Add(new SoundPlayerElement(go, volumeList, playLog));
            }
        }

        public void Update() {
            foreach(var element in elements) {
                element.Update();
            }
        }

        // 使用していないプレイヤーを取得、全て再生中なら0番目を取得、取得したプレイヤーはリスト最後尾に移動
        private SoundPlayerElement GetUnusedElement() {
            SoundPlayerElement target = null;
            foreach(SoundPlayerElement element in elements) {
                if(element.IsPlaying == false) {
                    target = element;
                    break;
                }
            }
            if(target == null) target = elements[0];
            // 最後尾に移動
            elements.Remove(target);
            elements.Add(target);
            return target;
        }

        public void Play(string soundId, bool loop) {
            GetUnusedElement().Play(soundId, loop);
        }
        public void Play(string soundId) {
            Play(soundId, defaultLoop);
        }

        public void Stop() {
            foreach(SoundPlayerElement element in elements) {
                element.Stop();
            }
        }
        public void Stop(string soundId) {
            elements.Find(x => x.SoundId == soundId)?.Stop();
        }

        public void Fade(float duration, float from, float to, UnityAction onFadeFinish = null) {
            foreach(SoundPlayerElement element in elements) {
                element?.Fade(duration, from, to, onFadeFinish);
            }
        }
        public void Fade(string soundId, float duration, float from, float to, UnityAction onFadeFinish = null) {
            SoundPlayerElement element = elements.Find(x => x.SoundId == soundId);
            element?.Fade(duration, from, to, onFadeFinish);
        }

        public void FadeIn(string soundId, float duration, bool loop) {
            SoundPlayerElement element = GetUnusedElement();
            element.Play(soundId, loop);
            element.Fade(duration, 0, 1);
        }
        public void FadeIn(string soundId, float duration) {
            FadeIn(soundId, duration, defaultLoop);
        }
        public void FadeIn(string soundId, bool loop) {
            FadeIn(soundId, defaultFadeDuration, loop);
        }
        public void FadeIn(string soundId) {
            FadeIn(soundId, defaultFadeDuration, defaultLoop);
        }


        public void FadeOut(float duration) {
            foreach(SoundPlayerElement element in elements) {
                element?.Fade(duration, 1, 0, () => element.Stop());
            }
        }
        public void FadeOut() {
            FadeOut(defaultFadeDuration);
        }
        public void FadeOut(string soundId, float duration) {
            SoundPlayerElement element = elements.Find(x => x.SoundId == soundId);
            element?.Fade(duration, 1, 0, () => element.Stop());
        }
        public void FadeOut(string soundId) {
            FadeOut(soundId, defaultFadeDuration);
        }

        public void CrossFade(string soundId, float duration, bool loop) {
            FadeOut(duration);
            FadeIn(soundId, duration, loop);
        }
        public void CrossFade(string soundId, float duration) {
            CrossFade(soundId, duration, defaultLoop);
        }
        public void CrossFade(string soundId, bool loop) {
            CrossFade(soundId, defaultFadeDuration, loop);
        }
        public void CrossFade(string soundId) {
            CrossFade(soundId, defaultFadeDuration, defaultLoop);
        }
    }
}