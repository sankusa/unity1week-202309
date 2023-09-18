using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    private Material material;

    [SerializeField] private Vector2 tiling = new Vector2(1, 1);
    [SerializeField] private Vector2 scrollVelocity;

    void Start() {
        if(TryGetComponent<Renderer>(out Renderer renderer)) {
            material = renderer.material;
        }
        if(material == null && TryGetComponent<Graphic>(out Graphic graphic)) {
            graphic.material = new Material(graphic.material);
            material = graphic.material;
        }
    }

    void Update() {
        material?.SetVector("_Tiling", tiling);
        material?.SetVector("_ScrollVelocity", scrollVelocity);
    }

    void OnDestroy() {
        Destroy(material);
        material = null;
    }
}
