using System;
using Core.Controllers;
using Core.Items;
using Core.Service;
using Settings;
using UnityEngine;
using Zenject;
using UnityEngine.UI;

namespace Core.UI
{
    public class StartPage : MonoBehaviour, IUIElement, IDisposable, ITickable
    {
        [Inject] private IAppStateService _appStateService;
        [Inject] private CameraController _cameraController;
        
        [SerializeField] private Button _startButton;
        
        private void OnValidate()
        {
            _startButton = transform.Find("Button_Start").GetComponent<Button>();
        }

        public void Init()
        {
            _startButton.onClick.AddListener(StartButtonClickedHandler);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Show(object data)
        {
            Show();
        } 

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Tick()
        {
            
        }

        public void Dispose()
        {
            _startButton.onClick.RemoveAllListeners();
        }

        private void StartButtonClickedHandler()
        {
            Hide();
            _cameraController.AnimateToCarView(()=> _appStateService.ChangeAppState(Enumerators.AppState.Game));
        }
    }
}