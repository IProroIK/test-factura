using Core.Service;
using Settings;
using UnityEngine;
using Zenject;

namespace Core.Items
{
    public class Finish : MonoBehaviour
    {
        [Inject] private IAppStateService _appStateService;

        private void OnTriggerEnter(Collider other)
        {
            if(!other.gameObject.TryGetComponent(out CarController carController))
                return;
            
            _appStateService.ChangeAppState(Enumerators.AppState.Win);
        }
    }
}