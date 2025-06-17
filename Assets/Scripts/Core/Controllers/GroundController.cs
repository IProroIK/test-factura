using System;
using Core.Items;
using Core.Service;
using Model;
using Settings;
using UnityEngine;
using Zenject;

namespace Core.Controllers
{
    public class GroundController : IInitializable, IDisposable
    {
        public event Action<Vector3> GroundPlacedEvent;
        
        private IPoolService _poolService;
        private IAppStateService _appStateService;
        
        private Ground _groundPrefab;
        private Finish _groundEndPrefab;
        private IPool<Ground> _groundPool;
        private float _currentOffset;
        
        private GroundData _groundData;
        private int _currentGroundIndex;
        private DiContainer _container;
        private LevelController _levelController;
        private const int InitialGroundLength = 2;

        public GroundController(IPoolService poolService, IAppStateService appStateService, DiContainer container, LevelController levelController)
        {
            _levelController = levelController;
            _container = container;
            _poolService = poolService;
            _appStateService = appStateService;
        }

        public void Initialize()
        {
            _groundData = _levelController.GetGroundData();
            _groundPrefab = _groundData.GroundPrefab;
            _groundEndPrefab = _groundData.EndGroundPrefab; 
            
            _poolService.CreatePool(_groundPrefab, 4);
            _groundPool = _poolService.GetPool<Ground>();
            
            _appStateService.AppStateChangedEvent += AppStateChangedEventHandler;
        }

        public void Dispose()
        {
            _appStateService.AppStateChangedEvent -= AppStateChangedEventHandler;    
        }
        
        private void SetInitialRoad()
        {
            for (int i = 0; i < InitialGroundLength; i++)
            {
                GroundTriggerEnteredEventHandler();
            }
        }

        private void GroundTriggerEnteredEventHandler()
        {
            if (_currentGroundIndex <= _groundData.LevelLength)
            {
                _currentOffset -= _groundData.GroundLength;
                _currentGroundIndex++;
                Vector3 spawnPosition = new Vector3(0, 0, _currentOffset);
                var ground = _groundPool.Spawn(spawnPosition, _groundPrefab.transform.rotation);
                
                if (ground is Ground groundObject)
                {
                    groundObject.GroundTriggerEnteredEvent += GroundTriggerEnteredEventHandler;
                }
                GroundPlacedEvent?.Invoke(ground.transform.position);
            }
            else
            {
                _container.InstantiatePrefabForComponent<Finish>(_groundEndPrefab, new Vector3(0, 0, _currentOffset),
                    _groundEndPrefab.transform.rotation, null);
            }
        }

        private void AppStateChangedEventHandler()
        {
            if (_appStateService.AppState == Enumerators.AppState.Game)
            {
                _currentOffset = 0;
                _currentGroundIndex = 2;
                SetInitialRoad();
            }
        }
    }
}