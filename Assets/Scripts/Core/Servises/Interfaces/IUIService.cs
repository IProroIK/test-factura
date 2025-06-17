using Core.Items;

namespace Core.Service
{
    public interface IUIService
    {
        void Register<T>(T element) where T : IUIElement;
        void Show<T>(object data = null) where T : IUIElement;
        void Hide<T>() where T : IUIElement;
        void HideAll();
    }
}