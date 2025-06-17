namespace Core.Items
{
    public interface IUIElement
    {
        void Init();
        void Show(object data = null);
        void Hide();
    }
}