using System;
using Core.Controllers;
using Core.Service;
using Core.View;
using Model;
using Settings;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Core.Items
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Enemy : MonoBehaviour, IDamageable, IPoolable
    {
        public event Action<IPoolable> Despawned;
        public event Action<Enemy> DeathEvent;
        public event Action<Vector3, float> DamagedEvent;

        private ICameraService _cameraService;
        private IAppStateService _appStateService;

        private float _idleBeforeWander;
        
        private Rigidbody _rigidbody;
        private EnemyView _enemyView;
        private EnemyModel _enemyModel;
        private Transform _target;
        private Material _material;

        private float _idleTimer;
        private bool _isWandering;
        private Vector3 _wanderTarget;
        private LevelController _levelController;

        [Inject]
        private void Constract(ICameraService cameraService, IAppStateService appStateService, LevelController levelController)
        {
            _levelController = levelController;
            _cameraService = cameraService;
            _appStateService = appStateService;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _enemyView = new EnemyView(transform, _cameraService);
            _enemyModel = new EnemyModel(_levelController.GetEnemyData());
            _enemyModel.Init();

            _appStateService.AppStateChangedEvent += AppStateChangedEventHandler; 
            
            _enemyModel.DeathEvent += DeathEventHandler;
        }

        private void OnEnable()
        {
            _enemyView.SetIdleAnimation();
            _enemyModel.SetStartValue();
            _idleTimer = 0f;
            _idleBeforeWander = Random.Range(5f, 15f);
            _isWandering = false;
        }

        public void Damage(float damage)
        {
            DamagedEvent?.Invoke(transform.position, damage);
            _enemyModel.Damage(damage);
            _enemyView.Damage(_enemyModel.GetNormalizedHealth());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out CarController carController))
                return;

            _target = other.transform;
            _isWandering = false;
            _enemyView.SetRunAnimation();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.TryGetComponent(out CarController carController))
                return;

            if (carController is IDamageable damageable)
                damageable.Damage(_enemyModel.GetDamage());

            DeathEventHandler();
        }

        private void OnDestroy()
        {
            _enemyModel.DeathEvent -= DeathEventHandler;
        }

        private void FixedUpdate()
        {
            if (_target != null)
            {
                _isWandering = false;
                RunToTarget();
            }
            else
            {
                HandleWander();
            }

            _enemyView.FixedUpdate();
        }

        private void RunToTarget()
        {
            Vector3 direction = _target.position - transform.position;
            direction.y = 0;

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }

            direction.Normalize();
            _rigidbody.velocity = direction * _enemyModel.GetSpeed();
        }

        private void HandleWander()
        {
            if (_isWandering)
            {
                RunWander();
                return;
            }

            _rigidbody.velocity = Vector3.zero;
            _idleTimer += Time.fixedDeltaTime;

            if (_idleTimer >= _idleBeforeWander)
                StartWander();
        }

        private void StartWander()
        {
            _idleTimer = 0f;
            _isWandering = true;

            _idleBeforeWander = Random.Range(5f, 15f);
            int direction = Random.Range(0, 2) == 0 ? -1 : 1;
            float distance = Random.Range(1f, 3f);
            _wanderTarget = transform.position + new Vector3(direction * distance, 0, 0);

            _enemyView.SetRunAnimation();
        }

        private void RunWander()
        {
            Vector3 direction = _wanderTarget - transform.position;
            direction.y = 0;

            if (direction.sqrMagnitude <= 0.05f)
            {
                StopWander();
                return;
            }

            direction.Normalize();
            _rigidbody.velocity = direction * _enemyModel.GetSpeed()/3;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        private void StopWander()
        {
            _isWandering = false;
            _idleTimer = 0f;
            _rigidbody.velocity = Vector3.zero;
            _enemyView.SetIdleAnimation();
        }

        private void DeathEventHandler()
        {
            _target = null;
            _isWandering = false;
            _idleTimer = 0f;

            Despawned?.Invoke(this);
            DeathEvent?.Invoke(this);
            DeathEvent = null;
        }

        private void AppStateChangedEventHandler()
        {
            if (_appStateService.AppState == Enumerators.AppState.Lose ||
                _appStateService.AppState == Enumerators.AppState.Win)
            {
                Despawned?.Invoke(this);
            }
        }
    }
}
