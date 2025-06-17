using System;
using Core.Items;
using Core.Service;
using Model;
using UnityEditor;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Core.Controllers
{
    public class EnemysController : IInitializable, IDisposable
    {
        private IPoolService _poolService;
        private GroundController _groundController;
        private EnemySpawnData _spawnData;
        
        private Enemy _enemyPrefab;
        private EnemyDeathEffect _deathEffectPrefab;
        private DamageText _damageTextPrefab;
        
        private IPool<Enemy> _enemyPool;
        private IPool<EnemyDeathEffect> _deathEffectPool;
        private IPool<DamageText> _textPool;
        private LevelController _levelController;

        public EnemysController(IPoolService poolService, GroundController groundController, LevelController levelController)
        {
            _levelController = levelController;
            _groundController = groundController;
            _poolService = poolService;
        }

        public void Initialize()
        {
            _deathEffectPrefab = Resources.Load<EnemyDeathEffect>("Prefabs/Gameplay/EnemyDeathEffect");
            _enemyPrefab = Resources.Load<Enemy>("Prefabs/Gameplay/Enemy");
            _damageTextPrefab = Resources.Load<DamageText>("Prefabs/Gameplay/DamageText");
            _spawnData = _levelController.GetEnemySpawnData();

            _poolService.CreatePool(_enemyPrefab, 200);
            _poolService.CreatePool(_deathEffectPrefab, 10);
            _poolService.CreatePool(_damageTextPrefab, 10);
            
            _enemyPool = _poolService.GetPool<Enemy>();
            _deathEffectPool = _poolService.GetPool<EnemyDeathEffect>();
            _textPool = _poolService.GetPool<DamageText>();
            
            _groundController.GroundPlacedEvent += GroundPlacedEventHandler;
            
        }

        public void Dispose()
        {
            _groundController.GroundPlacedEvent -= GroundPlacedEventHandler;
        }

        private void SpawnEnemy(Vector3 areaOrigin)
        {
            Vector3 size = _spawnData.SpawnAreaSize;
            Vector3 bottomRight = new Vector3(
                areaOrigin.x + size.x, 
                areaOrigin.y,         
                areaOrigin.z          
            );
            
            
            int countX = Mathf.FloorToInt(size.x / _spawnData.SpawnOffset.x);
            int countZ = Mathf.FloorToInt(size.z / _spawnData.SpawnOffset.z);
            
            for (int x = 0; x <= countX; x++)
            {
                for (int z = 0; z <= countZ; z++)
                {
                    Vector3 randomOffset = new Vector3(
                        Random.Range(-_spawnData.RandomOffset.x, _spawnData.RandomOffset.x),
                        Random.Range(-_spawnData.RandomOffset.y, _spawnData.RandomOffset.y),
                        Random.Range(-_spawnData.RandomOffset.z, _spawnData.RandomOffset.z)
                        );

                    Vector3 position = (areaOrigin - new Vector3(size.x/2, 0, size.z/2))
                                       + new Vector3(x * _spawnData.SpawnOffset.x, 0, z * _spawnData.SpawnOffset.z)
                                       + randomOffset;
                    position.y = areaOrigin.y;

                    Enemy enemy = null;
                    
                    if (_spawnData.IsRandomRotation)
                    { 
                        enemy = _enemyPool.Spawn(position, Quaternion.Euler(0, Random.Range(0f, 360f), 0));
                    }
                    else
                    {
                        enemy = _enemyPool.Spawn(position, Quaternion.identity);
                    }

                    enemy.DeathEvent += DeathEventHandler;
                    enemy.DamagedEvent += DamageEventHandler;
                }
            }
        }

        private void DamageEventHandler(Vector3 position, float damage)
        {
            var textEffect = _textPool.Spawn(position, _damageTextPrefab.transform.rotation);
            textEffect.AnimateText(damage);
        }

        private void DeathEventHandler(Enemy enemy)
        {
            enemy.DamagedEvent -= DamageEventHandler; 
            _deathEffectPool.Spawn(enemy.transform.position, Quaternion.identity);
        }

        private void GroundPlacedEventHandler(Vector3 position)
        {
            SpawnEnemy(position);
        }
    }
}