using System;
using UnityEngine;

namespace Eughc.Farm {
    public class PlantSystemHarvest : MonoBehaviour, IPlantComponent {
        public event Action OnHarvestEnded;

        private Plant _plant;
        private GameObject _fruitPrefab;
        private Vector3 _spawnPosition;

        private int _yield;
        private int _currentCycleYield;
        private int _currentCycleHarvested;

        public int RemainingFruits => _currentCycleYield - _currentCycleHarvested;

        public void Initialize(Plant plant) {
            _plant = plant;
            _spawnPosition = plant.transform.position;
            _fruitPrefab = plant.FruitPrefab;
            _yield = plant.Stats.EffectiveYield;

            plant.Stats.OnStatsRecomputed += () => _yield = _plant.Stats.EffectiveYield;
            plant.Lifecycle.OnStageChanged += def => { if (def.Name == PlantStage.Harvestable) NewCycle(); };
            plant.OnTick += OnTick;
        }

        private void OnTick(int tickNum) {
            if (_plant.Lifecycle.CurrentStage.Name != PlantStage.Harvestable) return;
            var progressLeft = 1f - _plant.Lifecycle.GetStageProgressNormalized();
            _currentCycleYield = Mathf.CeilToInt(progressLeft * _yield);
        }

        private Fruit SpawnFruit() {
            var go = UnityEngine.Object.Instantiate(_fruitPrefab, _spawnPosition, Quaternion.identity);
            go.TryGetComponent(out Fruit fruit);
            if (fruit == null) throw new MissingComponentException();
            return fruit;
        }

        public bool CanHarvest() => _currentCycleHarvested < _currentCycleYield;

        public void Harvest(FruitBasket basket) {
            while (CanHarvest() && basket.HasCapacity()) {
                basket.TryAdd(SpawnFruit());
                _currentCycleHarvested++;
            }

            if (!CanHarvest()) {
                OnHarvestEnded?.Invoke();
                _plant.Lifecycle.NextStage();
            }
        }

        public void NewCycle() => _currentCycleHarvested = 0;
    }
}