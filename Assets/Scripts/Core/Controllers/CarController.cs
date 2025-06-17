using System;
using Core.Controllers;
using Core.Items;
using Core.Service;
using Core.View;
using Model;
using Settings;
using UnityEngine;
using Zenject;

public class CarController : MonoBehaviour, IFixedTickable, IDamageable, IDisposable
{
    private IAppStateService _appStateService;
    private CameraController _cameraController;
    
    private Rigidbody _rigidbody;
    private CarModel _carModel;
    private CarView _carView;

    [Inject]
    private void Contstruct(IAppStateService appStateService, CameraController cameraController)
    {
        _appStateService = appStateService;
        _cameraController = cameraController;
    }
    
    private void Awake()
    {
        _carModel = new CarModel();
        _carModel.Init(transform);
        _carView = new CarView(transform, _cameraController);
        _carView.Init();
        
        _rigidbody = GetComponent<Rigidbody>();

        _carModel.DeathEvent += DeathEventHandler;
        _appStateService.AppStateChangedEvent += AppStateChangedEventHandler;
    }

    private void AppStateChangedEventHandler()
    {
        _rigidbody.velocity = Vector3.zero;

        if (_appStateService.AppState == Enumerators.AppState.Game)
        {
            _carModel.Reset();
            _carView.Reset();
        }
        
    }

    public void FixedTick()
    {
        if(_appStateService.AppState != Enumerators.AppState.Game)
            return;
            
        _rigidbody.velocity += transform.forward * (-1 * (_carModel.GetSpeed() * Time.fixedDeltaTime));

        if (_rigidbody.velocity.magnitude > _carModel.GetMaxSpeed())
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * _carModel.GetMaxSpeed();
        }
        
    }

    public void Damage(float damage)
    {
        _carModel.Damage(damage);
        
        _carView.Damage(_carModel.GetNormalizedHealth());
        
    }

    private void DeathEventHandler()
    {
        _carView.CarExplosion();
        
        _appStateService.ChangeAppState(Enumerators.AppState.Lose);
    }

    public void Dispose()
    {
        _carModel.DeathEvent -= DeathEventHandler;
    }
}
