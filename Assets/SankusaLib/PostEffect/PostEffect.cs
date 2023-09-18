using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib {
    [ExecuteAlways]
    public class PostEffect : MonoBehaviour
    {
        public Material material;
        void OnRenderImage(RenderTexture source, RenderTexture destination) {
            if(material != null) {
                Graphics.Blit(source, destination, material);
            } else {
                Graphics.Blit(source, destination);
            }
        }
    }
}