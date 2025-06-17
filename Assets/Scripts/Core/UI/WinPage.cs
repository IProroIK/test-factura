using Core.Items;
using Core.Service;
using Settings;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.UI
{
    public class WinPage : MonoBehaviour, IUIElement
    {
        [SerializeField] private Button _continueButton;
        private IAppStateService _appStateService;

        [Inject]
        private void Contruct(IAppStateService appStateService)
        {
            _appStateService = appStateService;
        }
        
        public void Init()
        {
            _continueButton.onClick.AddListener(ContinueButtonClickedHandler);            
        }

        private void ContinueButtonClickedHandler()
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