using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

namespace SankusaLib {
    [ExecuteAlways()]
    public class ColorSynchronizer : MonoBehaviour
    {
        [SerializeField] private Color color;
        [SerializeField] private List<Image> spriteRenderers;
        [SerializeField] private List<Image> images;
        [SerializeField] private List<TMP_Text> tmpTexts;
        private Color colorOld = Color.clear;

        // Start is called before the first frame update
        void Awake()
        {
            UpdateColor();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateColor();
        }

        private void UpdateColor() {
            if(colorOld != color) {
                spriteRenderers.Where(x => x != null).ToList().ForEach(x => x.color = color);
                images.Where(x => x != null).ToList().ForEach(x => x.color = color);
                tmpTexts.Where(x => x != null).ToList().ForEach(x => x.color = color);
                colorOld = color;
            }
        }
    }
}