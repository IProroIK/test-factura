using Core.Items;
using Core.Service;
using Core.View;
using DG.Tweening;
using Model;
using Settings;
using UnityEngine;
using Zenject;

namespace Core.Controllers
{
    public class TurretController : MonoBehaviour
    {
        private IInputService _inputService;
        private IAppStateService _appStateService;
        private TurretView _turretView;
        private TurretModel _turretModel;
        private TurretData _turretData;
        private IPoolService _poolService;
        private Bullet _bulletPrefab;
     
        [SerializeField] private Transform _shootPoint;
        
        [Inject]
        public void Construct(IInputService inputService, 
            IAppStateService appStateService, IPoolService poolService)
        {
            _poolService = poolService;
            _inputService = inputService;
            _appStateService = appStateService;
        }

        private void Awake()
        {
            _turretData = Resources.Load<TurretData>("Data/TurretData");
            _bulletPrefab = Resources.Load<Bullet>("Prefabs/Gameplay/bullet");
            _inputService.RegisterInputHandler(Enumerators.InputType.Mouse, 0, onInput: OnInputHandler);
            _turretView = new TurretView(transform);
            _turretModel = new TurretModel(_turretData);

            _poolService.CreatePool(_bulletPrefab, 50);
        }

        private void OnInputHandler()
        {
            if (_appStateService.AppState != Enumerators.AppState.Game)
                return;

            Vector2 pos = Input.mousePosition;
            _turretView.SetFromScreenPosition(pos.x);
            if (_turretModel.CanShoot())
            {
                var pool= _poolService.GetPool<Bullet>();
                var bullet = pool.Spawn(_shootPoint.position, Quaternion.identity);
                
                bullet.SetData(_turretModel.GetDamage());
                bullet.Shot(_shootPoint.forward.normalized);
                
                _turretModel.RegisterShot();
            }
        }
    }
}