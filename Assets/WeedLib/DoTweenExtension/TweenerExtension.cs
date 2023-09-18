using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace WeedLib
{
    public static class TweenerExtension
    {
        // 再生終了後のTweenerのIsPlayingにアクセスしようとすると警告が発生するので、先にIsActiveを確認しなければいけない。
        // 使うケースが多いため、シンプルに書けるように拡張メソッド化。tweener?.SafeKill()と書けば済む。
        public static void SafeKill(this Tweener tweener)
        {
            if(tweener != null && tweener.IsActive() && tweener.IsPlaying())
            {
                tweener.Kill();
            }
        }
    }
}