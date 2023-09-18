using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SankusaLib {
    public class SimpleDialogBox : MonoBehaviour
    {
        [SerializeField] private Text messageText;
        [SerializeField] private Button selectionButton1;
        [SerializeField] private Text selectionText1;
        [SerializeField] private Button selectionButton2;
        [SerializeField] private Text selectionText2;

        void Awake() {
            selectionButton1.onClick.AddListener(() => Destroy(gameObject));
            selectionButton2.onClick.AddListener(() => Destroy(gameObject));
        }

        public void SetUp(string message, string selection1, string selection2, UnityAction callback1, UnityAction callback2) {
            messageText.text = message;
            selectionText1.text = selection1;
            selectionText2.text = selection2;
            if(callback1 != null) selectionButton1.onClick.AddListener(callback1);
            if(callback2 != null) selectionButton2.onClick.AddListener(callback2);
        }
        public void SetUp(string message, string selection1, UnityAction callback1) {
            messageText.text = message;
            selectionText1.text = selection1;
            if(callback1 != null) selectionButton1.onClick.AddListener(callback1);

            // 選択肢1を中心に、選択肢2を非アクティブに
            Vector3 selection1Pos = selectionButton1.transform.localPosition;
            selection1Pos.x = 0;
            selectionButton1.transform.localPosition = selection1Pos;

            selectionButton2.gameObject.SetActive(false);
        }
    }
}