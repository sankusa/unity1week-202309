
using System;
using UnityEngine;
using UnityEngine.Events;
namespace SankusaLib.SoundLib {
    public partial class SoundManagerAccessor : MonoBehaviour {

        public float MasterVolume {
            get => SoundManager.Instance.FindVolume("Master").Value;
            set => SoundManager.Instance.FindVolume("Master").Value = value;
        }
        public bool MasterMute {
            get => SoundManager.Instance.FindVolume("Master").Mute;
            set => SoundManager.Instance.FindVolume("Master").Mute = value;
        }
        public IObservable<float> OnMasterVolumeChanged => SoundManager.Instance.FindVolume("Master").OnValueChanged;
        public IObservable<bool> OnMasterMuteChanged => SoundManager.Instance.FindVolume("Master").OnMuteChanged;

        public float BgmVolume {
            get => SoundManager.Instance.FindVolume("Bgm").Value;
            set => SoundManager.Instance.FindVolume("Bgm").Value = value;
        }
        public bool BgmMute {
            get => SoundManager.Instance.FindVolume("Bgm").Mute;
            set => SoundManager.Instance.FindVolume("Bgm").Mute = value;
        }
        public IObservable<float> OnBgmVolumeChanged => SoundManager.Instance.FindVolume("Bgm").OnValueChanged;
        public IObservable<bool> OnBgmMuteChanged => SoundManager.Instance.FindVolume("Bgm").OnMuteChanged;

        public float SeVolume {
            get => SoundManager.Instance.FindVolume("Se").Value;
            set => SoundManager.Instance.FindVolume("Se").Value = value;
        }
        public bool SeMute {
            get => SoundManager.Instance.FindVolume("Se").Mute;
            set => SoundManager.Instance.FindVolume("Se").Mute = value;
        }
        public IObservable<float> OnSeVolumeChanged => SoundManager.Instance.FindVolume("Se").OnValueChanged;
        public IObservable<bool> OnSeMuteChanged => SoundManager.Instance.FindVolume("Se").OnMuteChanged;

        public void PlayBgm(string soundId, bool loop) {
            SoundManager.Instance.FindPlayer("Bgm").Play(soundId, loop);
        }
        public void PlayBgm(string soundId) {
            SoundManager.Instance.FindPlayer("Bgm").Play(soundId);
        }
        public void StopBgm() {
            SoundManager.Instance.FindPlayer("Bgm").Stop();
        }
        public void StopBgm(string soundId) {
            SoundManager.Instance.FindPlayer("Bgm").Stop(soundId);
        }
        public void FadeBgm(float duration, float from, float to, UnityAction onFadeFinish = null) {
            SoundManager.Instance.FindPlayer("Bgm").Fade(duration, from, to, onFadeFinish);
        }
        public void FadeBgm(string soundId, float duration, float from, float to, UnityAction onFadeFinish = null) {
            SoundManager.Instance.FindPlayer("Bgm").Fade(soundId, duration, from, to, onFadeFinish);
        }
        public void FadeInBgm(string soundId, float duration, bool loop) {
            SoundManager.Instance.FindPlayer("Bgm").FadeIn(soundId, duration, loop);
        }
        public void FadeInBgm(string soundId, float duration) {
            SoundManager.Instance.FindPlayer("Bgm").FadeIn(soundId, duration);
        }
        public void FadeInBgm(string soundId, bool loop) {
            SoundManager.Instance.FindPlayer("Bgm").FadeIn(soundId, loop);
        }
        public void FadeInBgm(string soundId) {
            SoundManager.Instance.FindPlayer("Bgm").FadeIn(soundId);
        }
        public void FadeOutBgm(float duration) {
            SoundManager.Instance.FindPlayer("Bgm").FadeOut(duration);
        }
        public void FadeOutBgm() {
            SoundManager.Instance.FindPlayer("Bgm").FadeOut();
        }
        public void FadeOutBgm(string soundId, float duration) {
            SoundManager.Instance.FindPlayer("Bgm").FadeOut(soundId, duration);
        }
        public void FadeOutBgm(string soundId) {
            SoundManager.Instance.FindPlayer("Bgm").FadeOut(soundId);
        }
        public void CrossFadeBgm(string soundId, float duration, bool loop) {
            SoundManager.Instance.FindPlayer("Bgm").CrossFade(soundId, duration, loop);
        }
        public void CrossFadeBgm(string soundId, float duration) {
            SoundManager.Instance.FindPlayer("Bgm").CrossFade(soundId, duration);
        }
        public void CrossFadeBgm(string soundId, bool loop) {
            SoundManager.Instance.FindPlayer("Bgm").CrossFade(soundId, loop);
        }
        public void CrossFadeBgm(string soundId) {
            SoundManager.Instance.FindPlayer("Bgm").CrossFade(soundId);
        }

        public void PlaySe(string soundId, bool loop) {
            SoundManager.Instance.FindPlayer("Se").Play(soundId, loop);
        }
        public void PlaySe(string soundId) {
            SoundManager.Instance.FindPlayer("Se").Play(soundId);
        }
        public void StopSe() {
            SoundManager.Instance.FindPlayer("Se").Stop();
        }
        public void StopSe(string soundId) {
            SoundManager.Instance.FindPlayer("Se").Stop(soundId);
        }
        public void FadeSe(float duration, float from, float to, UnityAction onFadeFinish = null) {
            SoundManager.Instance.FindPlayer("Se").Fade(duration, from, to, onFadeFinish);
        }
        public void FadeSe(string soundId, float duration, float from, float to, UnityAction onFadeFinish = null) {
            SoundManager.Instance.FindPlayer("Se").Fade(soundId, duration, from, to, onFadeFinish);
        }
        public void FadeInSe(string soundId, float duration, bool loop) {
            SoundManager.Instance.FindPlayer("Se").FadeIn(soundId, duration, loop);
        }
        public void FadeInSe(string soundId, float duration) {
            SoundManager.Instance.FindPlayer("Se").FadeIn(soundId, duration);
        }
        public void FadeInSe(string soundId, bool loop) {
            SoundManager.Instance.FindPlayer("Se").FadeIn(soundId, loop);
        }
        public void FadeInSe(string soundId) {
            SoundManager.Instance.FindPlayer("Se").FadeIn(soundId);
        }
        public void FadeOutSe(float duration) {
            SoundManager.Instance.FindPlayer("Se").FadeOut(duration);
        }
        public void FadeOutSe() {
            SoundManager.Instance.FindPlayer("Se").FadeOut();
        }
        public void FadeOutSe(string soundId, float duration) {
            SoundManager.Instance.FindPlayer("Se").FadeOut(soundId, duration);
        }
        public void FadeOutSe(string soundId) {
            SoundManager.Instance.FindPlayer("Se").FadeOut(soundId);
        }
        public void CrossFadeSe(string soundId, float duration, bool loop) {
            SoundManager.Instance.FindPlayer("Se").CrossFade(soundId, duration, loop);
        }
        public void CrossFadeSe(string soundId, float duration) {
            SoundManager.Instance.FindPlayer("Se").CrossFade(soundId, duration);
        }
        public void CrossFadeSe(string soundId, bool loop) {
            SoundManager.Instance.FindPlayer("Se").CrossFade(soundId, loop);
        }
        public void CrossFadeSe(string soundId) {
            SoundManager.Instance.FindPlayer("Se").CrossFade(soundId);
        }

    }
}
