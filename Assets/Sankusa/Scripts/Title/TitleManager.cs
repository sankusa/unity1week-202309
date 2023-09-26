using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Sankusa.unity1week202309.InputManagement;
using Sankusa.unity1week202309.Scene;

namespace Sankusa.unity1week202309.Title
{
    public class TitleManager : MonoBehaviour
    {
        [Inject] private IInputProvider _inputProvider;
        void Update()
        {
            if(_inputProvider.GetMainButtonDown())
            {
                SceneLoader.LoadInGameScene();
            }
        }
    }
}