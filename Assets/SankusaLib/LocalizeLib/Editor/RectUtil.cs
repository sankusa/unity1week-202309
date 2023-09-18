using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SankusaLib.LocalizeLib {
    public class RectUtil
    {
        // 左右のマージン
        public static Rect HorizontalMargin(Rect rect, float leftMargin = 0, float rightMargin = 0) {
            return new Rect(rect.x + leftMargin, rect.y, rect.width - leftMargin - rightMargin, rect.height);
        }
        // Rect分割
        public static List<Rect> DivideRect(Rect rect, List<float> widthList, List<int> notExpandIndexList, float leftMargin = 0, float rightMargin = 0) {
            // notExpandIndexListがnullの場合、空のリストとして扱う
            if(notExpandIndexList == null) notExpandIndexList = new List<int>();
            // 固定幅/非固定幅それぞれの幅の合計
            float notExpandWidthTotal = widthList.Where((x, i) => notExpandIndexList.IndexOf(i) != -1).Sum();
            float expandWidthTotal = widthList.Where((x, i) => notExpandIndexList.IndexOf(i) == -1).Sum();

            // 調整後の幅
            List<float> expandedWidthList = new List<float>();
            for(int i = 0; i < widthList.Count; i++) {
                if(notExpandIndexList.IndexOf(i) != -1) {
                    expandedWidthList.Add(widthList[i]);
                } else {
                    expandedWidthList.Add((rect.width - notExpandWidthTotal) * widthList[i] / expandWidthTotal);
                }
            }
            // Rect生成
            List<Rect> rects = new List<Rect>();
            for(int i = 0; i < expandedWidthList.Count; i++) {
                float totalWidthToIndex = expandedWidthList.Take(i).Sum();
                rects.Add(new Rect(rect.x + totalWidthToIndex + leftMargin, rect.y, expandedWidthList[i] - leftMargin - rightMargin, rect.height));
            }
            return rects;
        }
    }
}