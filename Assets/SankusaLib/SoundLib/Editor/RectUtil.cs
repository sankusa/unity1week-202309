using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SankusaLib.SoundLib {
    public class RectUtil
    {
        // 左右のマージン
        public static Rect Margin(Rect rect, float leftMargin = 0, float rightMargin = 0, float topMargin = 0, float bottomMargin = 0) {
            return new Rect(rect.x + leftMargin, rect.y + topMargin, rect.width - leftMargin - rightMargin, rect.height - topMargin - bottomMargin);
        }
        // Rect分割
        public static List<Rect> DivideRectHorizontal(Rect rect, List<float> widthList, List<int> notExpandIndexList, float leftMargin = 0, float rightMargin = 0) {
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
        
        public static List<Rect> DivideRectVertical(Rect rect, List<float> heightList, List<int> notExpandIndexList, float topMargin = 0, float bottomMargin = 0) {
            // notExpandIndexListがnullの場合、空のリストとして扱う
            if(notExpandIndexList == null) notExpandIndexList = new List<int>();
            // 固定幅/非固定幅それぞれの幅の合計
            float notExpandHeightTotal = heightList.Where((x, i) => notExpandIndexList.IndexOf(i) != -1).Sum();
            float expandHeightTotal = heightList.Where((x, i) => notExpandIndexList.IndexOf(i) == -1).Sum();

            // 調整後の幅
            List<float> expandedHeightList = new List<float>();
            for(int i = 0; i < heightList.Count; i++) {
                if(notExpandIndexList.IndexOf(i) != -1) {
                    expandedHeightList.Add(heightList[i]);
                } else {
                    expandedHeightList.Add((rect.height - notExpandHeightTotal) * heightList[i] / expandHeightTotal);
                }
            }
            // Rect生成
            List<Rect> rects = new List<Rect>();
            for(int i = 0; i < expandedHeightList.Count; i++) {
                float totalHeightToIndex = expandedHeightList.Take(i).Sum();
                rects.Add(new Rect(rect.x, rect.y + totalHeightToIndex + topMargin, rect.width , expandedHeightList[i] - topMargin - bottomMargin));
            }
            return rects;
        }
    }
}