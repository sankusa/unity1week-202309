using UnityEngine;
using Zenject;
using Sankusa.unity1week202309.InputManagement;

namespace Sankusa.unity1week202309
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<KeyboardInputProvider>()
                .AsSingle()
                .NonLazy();
        }
    }
}