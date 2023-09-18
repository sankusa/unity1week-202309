Shader "SankusaLib/Practice" {
    // Prperties : Unity側(インスペクタ)で入力する変数(全7型)
    Properties {
        // "_MainTex"はUnityがメインテクスチャと判定する特別な名前。他の名前のプロパティ名を
        //使いたければ、[MainTexture]というShaderLab属性を使う必要がある。
        _MainTex("Texture", 2D) = "" {}
        _NoUseRect("NoUseRect", Rect) = "white" {}
        _NoUseCube("NoUseCube", Cube) = "white" {}
        _NoUseRange("NoUseRange", Range(0.0, 1.0)) = 0.5
        _NoUseColor("NoUseColor", Color) = (0.5, 1.0, 0.1, 0.7)
        _NoUseFloat("NoUseFloat", Float) = 20.0
        _NoUseVector("NoUseVector", Vector) = (0.5, 0.7, 0.2, 0.4)
    }

    // シェーダーの本文
    SubShader {
        Pass {
            
        }
    }

    // SubShaderを上から試し、プラットフォームで動くシェーダーが無かった場合、
    // どのプラットフォームでも動く最終シェーダーをここで指定する
    Fallback "Diffuse"
}