
using System;
using UnityEngine.Events;
namespace SankusaLib.SoundLib {
    public partial class SoundManager {

        public float MasterVolume {
            get => FindVolume("Master").Value;
            set => FindVolume("Master").Value = value;
        }
        public bool MasterMute {
            get => FindVolume("Master").Mute;
            set => FindVolume("Master").Mute = value;
        }
        public IObservable<float> OnMasterVolumeChanged => FindVolume("Master").OnValueChanged;
        public IObservable<bool> OnMasterMuteChanged => FindVolume("Master").OnMuteChanged;

        public float BgmVolume {
            get => FindVolume("Bgm").Value;
            set => FindVolume("Bgm").Value = value;
        }
        public bool BgmMute {
            get => FindVolume("Bgm").Mute;
            set => FindVolume("Bgm").Mute = value;
        }
        public IObservable<float> OnBgmVolumeChanged => FindVolume("Bgm").OnValueChanged;
        public IObservable<bool> OnBgmMuteChanged => FindVolume("Bgm").OnMuteChanged;

        public float BgmSubVolume {
            get => FindVolume("BgmSub").Value;
            set => FindVolume("BgmSub").Value = value;
        }
        public bool BgmSubMute {
            get => FindVolume("BgmSub").Mute;
            set => FindVolume("BgmSub").Mute = value;
        }
        public IObservable<float> OnBgmSubVolumeChanged => FindVolume("BgmSub").OnValueChanged;
        public IObservable<bool> OnBgmSubMuteChanged => FindVolume("BgmSub").OnMuteChanged;

        public float SeVolume {
            get => FindVolume("Se").Value;
            set => FindVolume("Se").Value = value;
        }
        public bool SeMute {
            get => FindVolume("Se").Mute;
            set => FindVolume("Se").Mute = value;
        }
        public IObservable<float> OnSeVolumeChanged => FindVolume("Se").OnValueChanged;
        public IObservable<bool> OnSeMuteChanged => FindVolume("Se").OnMuteChanged;

        public float SeSubVolume {
            get => FindVolume("SeSub").Value;
            set => FindVolume("SeSub").Value = value;
        }
        public bool SeSubMute {
            get => FindVolume("SeSub").Mute;
            set => FindVolume("SeSub").Mute = value;
        }
        public IObservable<float> OnSeSubVolumeChanged => FindVolume("SeSub").OnValueChanged;
        public IObservable<bool> OnSeSubMuteChanged => FindVolume("SeSub").OnMuteChanged;

        public void PlayBgm(string soundId, bool loop) {
            FindPlayer("Bgm").Play(soundId, loop);
        }
        public void PlayBgm(string soundId) {
            FindPlayer("Bgm").Play(soundId);
        }
        public void StopBgm() {
            FindPlayer("Bgm").Stop();
        }
        public void StopBgm(string soundId) {
            FindPlayer("Bgm").Stop(soundId);
        }
        public void FadeBgm(float duration, float from, float to, UnityAction onFadeFinish = null) {
            FindPlayer("Bgm").Fade(duration, from, to, onFadeFinish);
        }
        public void FadeBgm(string soundId, float duration, float from, float to, UnityAction onFadeFinish = null) {
            FindPlayer("Bgm").Fade(soundId, duration, from, to, onFadeFinish);
        }
        public void FadeInBgm(string soundId, float duration, bool loop) {
            FindPlayer("Bgm").FadeIn(soundId, duration, loop);
        }
        public void FadeInBgm(string soundId, float duration) {
            FindPlayer("Bgm").FadeIn(soundId, duration);
        }
        public void FadeInBgm(string soundId, bool loop) {
            FindPlayer("Bgm").FadeIn(soundId, loop);
        }
        public void FadeInBgm(string soundId) {
            FindPlayer("Bgm").FadeIn(soundId);
        }
        public void FadeOutBgm(float duration) {
            FindPlayer("Bgm").FadeOut(duration);
        }
        public void FadeOutBgm() {
            FindPlayer("Bgm").FadeOut();
        }
        public void FadeOutBgm(string soundId, float duration) {
            FindPlayer("Bgm").FadeOut(soundId, duration);
        }
        public void FadeOutBgm(string soundId) {
            FindPlayer("Bgm").FadeOut(soundId);
        }
        public void CrossFadeBgm(string soundId, float duration, bool loop) {
            FindPlayer("Bgm").CrossFade(soundId, duration, loop);
        }
        public void CrossFadeBgm(string soundId, float duration) {
            FindPlayer("Bgm").CrossFade(soundId, duration);
        }
        public void CrossFadeBgm(string soundId, bool loop) {
            FindPlayer("Bgm").CrossFade(soundId, loop);
        }
        public void CrossFadeBgm(string soundId) {
            FindPlayer("Bgm").CrossFade(soundId);
        }

        public void PlaySe(string soundId, bool loop) {
            FindPlayer("Se").Play(soundId, loop);
        }
        public void PlaySe(string soundId) {
            FindPlayer("Se").Play(soundId);
        }
        public void StopSe() {
            FindPlayer("Se").Stop();
        }
        public void StopSe(string soundId) {
            FindPlayer("Se").Stop(soundId);
        }
        public void FadeSe(float duration, float from, float to, UnityAction onFadeFinish = null) {
            FindPlayer("Se").Fade(duration, from, to, onFadeFinish);
        }
        public void FadeSe(string soundId, float duration, float from, float to, UnityAction onFadeFinish = null) {
            FindPlayer("Se").Fade(soundId, duration, from, to, onFadeFinish);
        }
        public void FadeInSe(string soundId, float duration, bool loop) {
            FindPlayer("Se").FadeIn(soundId, duration, loop);
        }
        public void FadeInSe(string soundId, float duration) {
            FindPlayer("Se").FadeIn(soundId, duration);
        }
        public void FadeInSe(string soundId, bool loop) {
            FindPlayer("Se").FadeIn(soundId, loop);
        }
        public void FadeInSe(string soundId) {
            FindPlayer("Se").FadeIn(soundId);
        }
        public void FadeOutSe(float duration) {
            FindPlayer("Se").FadeOut(duration);
        }
        public void FadeOutSe() {
            FindPlayer("Se").FadeOut();
        }
        public void FadeOutSe(string soundId, float duration) {
            FindPlayer("Se").FadeOut(soundId, duration);
        }
        public void FadeOutSe(string soundId) {
            FindPlayer("Se").FadeOut(soundId);
        }
        public void CrossFadeSe(string soundId, float duration, bool loop) {
            FindPlayer("Se").CrossFade(soundId, duration, loop);
        }
        public void CrossFadeSe(string soundId, float duration) {
            FindPlayer("Se").CrossFade(soundId, duration);
        }
        public void CrossFadeSe(string soundId, bool loop) {
            FindPlayer("Se").CrossFade(soundId, loop);
        }
        public void CrossFadeSe(string soundId) {
            FindPlayer("Se").CrossFade(soundId);
        }

    }
}
