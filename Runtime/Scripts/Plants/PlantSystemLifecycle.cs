using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Eughc.Farm {
    public class PlantSystemLifecycle : MonoBehaviour, IPlantComponent {
        public event Action<PlantStageDefinition> OnStageChanged;

        private Plant _plant;
        private List<PlantStageDefinition> _lifecycle;
        public List<PlantStageDefinition> Lifecycle => _lifecycle;

        private PlantStageDefinition _currentStage;
        public PlantStageDefinition CurrentStage => _currentStage;
        public PlantStageDefinition InitialStage => _lifecycle[0];

        private float _stageProgress = 0f;
        public float Progress => _stageProgress;
        public float ProgressDuration => _currentStage.Duration;

        public float GetStageProgressNormalized() => _stageProgress / _currentStage.Duration;

        public PlantActiveEffect ActiveEffect {
            get {
                var info = PlantStageRegistrySO.Instance?.Get(_currentStage.Name);
                return new PlantActiveEffect(
                    info?.DisplayName ?? _currentStage.Name.ToString(),
                    info?.Icon,
                    _currentStage.GetStatModifierBundle()
                );
            }
        }

        public void Initialize(Plant plant) {
            _plant = plant;
            _lifecycle = plant.Definition.LifecycleStages.OrderBy(s => (int)s.Name).ToList();
            _currentStage = _lifecycle[0];
            _stageProgress = 0f;

            plant.OnTick += OnTick;
        }

        private void OnTick(int tickNum) {
            Tick(Mathf.Max(0, _plant.Stats.EffectiveGrowthRate));
        }

        private void ChangeStage(PlantStageDefinition newStage) {
            _currentStage = newStage;
            _stageProgress = 0f;
            _plant.Stats.SetBundle("stage", newStage.GetStatModifierBundle());
            OnStageChanged?.Invoke(_currentStage);
        }

        public void Tick(float amount) {
            _stageProgress += amount;
            if (_stageProgress < _currentStage.Duration) return;
            NextStage();
        }

        public void SetStage(PlantStage stage) {
            var definition = _lifecycle.FirstOrDefault(s => s.Name == stage);
            if (definition == null) return;
            ChangeStage(definition);
        }

        public void NextStage() {
            var nextStageName = PlantStageFSM.GetNextStage(_currentStage.Name, _lifecycle);
            if (nextStageName == null) return;
            var nextStage = _lifecycle.FirstOrDefault(s => s.Name == nextStageName);
            ChangeStage(nextStage);
        }

        public void ResetStageProgress() => ChangeStage(_currentStage);
    }
}