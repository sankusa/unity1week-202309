using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeedLib
{
    /// <summary>
    /// リスト型UIの管理用クラス
    /// </summary>
    /// <typeparam name="UI"></typeparam>
    public abstract class UIListManagerBase<UI> : MonoBehaviour where UI : MonoBehaviour
    {
        [SerializeField] protected UI uiPrefab;
        [SerializeField] protected Transform uiParent;

        protected List<UI> uiList = new List<UI>();
        protected List<UI> uiPool = new List<UI>();

        protected void UpdateUIList(int uiCount)
        {
            while(uiList.Count < uiCount)
            {
                UI newUi;
                if(uiPool.Count > 0)
                {
                    // プールにストックがあればそれを使う
                    newUi = uiPool[0];
                    uiPool.RemoveAt(0);
                    uiList.Add(newUi);
                }
                else
                {
                    // 新規生成
                    newUi = InstantiatePrefab(uiPrefab, uiParent);
                    uiList.Add(newUi);
                }
            }

            // UI更新
            for(int i = 0; i < uiCount; i++)
            {
                UpdateUIElement(i);
                uiList[i].gameObject.SetActive(true);
            }

            // 余剰分はクリアしてプールに追加
            int uiListCount = uiList.Count;
            for(int i = uiCount; i < uiListCount; i++)
            {
                ClearUIElement(uiList[uiCount]);
                uiList[uiCount].gameObject.SetActive(false);
                uiPool.Add(uiList[uiCount]);
                uiList.RemoveAt(uiCount);
            }
        }

        protected virtual UI InstantiatePrefab(UI uiPrefab, Transform uiParent)
        {
            return Instantiate(uiPrefab, uiParent);
        }
        protected abstract void UpdateUIElement(int index);
        protected virtual void ClearUIElement(UI uiElement) {}
    }
}