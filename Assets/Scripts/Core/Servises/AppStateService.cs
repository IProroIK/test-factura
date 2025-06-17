using System;
using Core.UI;
using Settings;
using UnityEngine;
using Zenject;

namespace Core.Service
{
    public class AppStateService : IAppStateService
    {
        public event Action AppStateChangedEvent;

        public Enumerators.AppState AppState { get; private set; } = Enumerators.AppState.Unknown;

        [Inject] private IUIService _uiService;

        public void ChangeAppState(Enumerators.AppState stateTo)
        {
            if (AppState == stateTo)
                return;
            
            AppState = stateTo;

            switch (stateTo)
            {
                case Enumerators.AppState.Main:
                    _uiService.Show<StartPage>();
                    break;
                case Enumerators.AppState.Game:
                    break;
                case Enumerators.AppState.Lose:
                    _uiService.Show<LosePage>();
                    break;
                case Enumerators.AppState.Win:
                    _uiService.Show<WinPage>();
                    break;
            }

            AppStateChangedEvent?.Invoke();
        }
    }
}