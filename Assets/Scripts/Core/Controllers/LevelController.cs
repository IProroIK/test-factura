using Core.Service;
using Model;
using Settings;
using UnityEngine;
using Zenject;

namespace Core.Controllers
{
    public class LevelController : IInitializable
    {
        private IAppStateService _appStateService;
        public int CurrentLevel { get; private set; }
        private LevelsData _levelData;
        
        public LevelController(IAppStateService appStateService)
        {
            _appStateService = appStateService;
        }
        
        public void Initialize()
        {
            _appStateService.AppStateChangedEvent += AppStateChangedEventHandler;
            _levelData = Resources.Load<LevelsData>("Data/LevelsData");
        }

        public EnemySpawnData GetEnemySpawnData()
        {
            return _levelData.EnemySpawnsData[CurrentLevel];
        }

        public EnemyData GetEnemyData()
        {
            return _levelData.EnemiesData[CurrentLevel];
        }

        public GroundData GetGroundData()
        {
            return _levelData.GroundsData[CurrentLevel];
        }

        private void AppStateChangedEventHandler()
        {
            if (_appStateService.AppState == Enumerators.AppState.Win)
            {
                CurrentLevel++;
                CurrentLevel = Mathf.Clamp(CurrentLevel, 0, _levelData.MaxLevelIndex); 
            }
        }
    }
}