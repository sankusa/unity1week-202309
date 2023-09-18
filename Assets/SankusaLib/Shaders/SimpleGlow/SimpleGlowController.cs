using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGlowController : MonoBehaviour
{
    private Material material;
    [ColorUsage(true, true), SerializeField] Color emissionColor;

    void Start() {
        material = GetComponent<Renderer>()?.material;
    }

    void Update() {
        material?.SetColor("_EmissionColor", emissionColor);
    }
}
