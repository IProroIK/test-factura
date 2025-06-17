using Core.Service;
using Core.UI;
using UnityEngine;
using Zenject;

namespace Core.Bindings
{
    public class UIBindingInstaller : MonoInstaller
    {
        [SerializeField] private StartPage _startPage;
        [SerializeField] private WinPage _winPage;
        [SerializeField] private LosePage _losePage;
        
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<StartPage>().FromInstance(_startPage).AsSingle();
            Container.BindInterfacesTo<WinPage>().FromInstance(_winPage).AsSingle();
            Container.BindInterfacesTo<LosePage>().FromInstance(_losePage).AsSingle();
            

            Container.Resolve<IUIService>().Register<StartPage>(_startPage);
            Container.Resolve<IUIService>().Register<WinPage>(_winPage);
            Container.Resolve<IUIService>().Register<LosePage>(_losePage);
        }
    }
}