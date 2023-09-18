using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreyEngine.Basic.Utils;

namespace GreyEngine.Basic.EditorResources {
    public class EditorIcons : ScriptableObject
    {
        public static readonly string PATH = "Assets/GreyEngine/Basic/EditorResources/Editor/Icons.asset";
        private static EditorIcons instance;
        public static EditorIcons Instance {
            get {
                if(instance == null) {
                    instance = AssetUtil.SafeLoadScriptableObject<EditorIcons>(PATH);
                }
                return instance;
            }
        }
        [SerializeField] public Texture2D upIcon;
        [SerializeField] public Texture2D downIcon;
        [SerializeField] public Texture2D plusIcon;
        [SerializeField] public Texture2D minusIcon;
        [SerializeField] public Texture2D copyIcon;
        [SerializeField] public Texture2D reloadIcon;
    }
}