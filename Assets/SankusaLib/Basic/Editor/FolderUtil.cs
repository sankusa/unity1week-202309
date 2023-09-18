using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace SankusaLib {
    public class FolderUtil {
        // ディレクトリが無ければ作成
        public static void SafeCreateDirectory(string directoryPath) {
            string currentPath = "";
            char[] splitChar = new char[] {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar};

            foreach(string directoryName in directoryPath.Split(splitChar)) {
                string parent = currentPath;
                currentPath = Path.Combine(currentPath, directoryName);
                if(!AssetDatabase.IsValidFolder(currentPath)) {
                    AssetDatabase.CreateFolder(parent, directoryName);
                }
            }
        }
    }
}