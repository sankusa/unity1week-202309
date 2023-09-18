using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib
{
    public static class MathUtil
    {
        public static float GetRandomByNormalDistribution(float average, float sigma, float normalizedRandom1, float normalizedRandom2)
        {
            float fixedNormalizedRandom1 = normalizedRandom1 != 0 ? normalizedRandom1 : 0.5f;
            return sigma * Mathf.Sqrt(-2.0f * Mathf.Log(fixedNormalizedRandom1)) * Mathf.Cos(2.0f * Mathf.PI * normalizedRandom2) + average; 
        }
    }
}