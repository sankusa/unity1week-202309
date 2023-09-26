using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SankusaLib;

namespace Sankusa.unity1week202309.Scene
{
    public class SceneLoader
    {
        private static void LoadScene(string sceneName)
        {
            Blackout.Instance.PlayBlackout(1, () =>
                {
                    SceneManager.LoadScene(sceneName);
                });
        }

        public static void LoadTitleScene()
        {
            LoadScene(SceneName.SCENE_NAME_TITLE);
        }

        public static void LoadInGameScene()
        {
            LoadScene(SceneName.SCENE_NAME_INGAME);
        }
    }
}