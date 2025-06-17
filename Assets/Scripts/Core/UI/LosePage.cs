using Core.Items;
using Core.Service;
using Settings;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.UI
{
    public class LosePage : MonoBehaviour, IUIElement
    {
        [SerializeField] private Button _restartButton;
        private IAppStateService _appStateService;
        
        [Inject]
        private void Contruct(IAppStateService appStateService)
        {
            _appStateService = appStateService;
        }
        
        public void Init()
        {
            _restartButton.onClick.AddListener(RestartButtonClickedHandler);            
        }

        private void RestartButtonClickedHandler()
        {
            Hide();
            _appStateService.ChangeAppState(Enumerators.AppState.Main);
        }

        public void Show(object data = null)
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}