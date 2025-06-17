using System;
using Settings;

namespace Core.Service
{
    public interface IAppStateService
    {
        event Action AppStateChangedEvent;
        public Enumerators.AppState AppState { get; }
        void ChangeAppState(Enumerators.AppState stateTo);
    }
}