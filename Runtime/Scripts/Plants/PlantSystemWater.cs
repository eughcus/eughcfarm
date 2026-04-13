using UnityEngine;

namespace Eughc.Farm {
    public class PlantSystemWater : MonoBehaviour, IPlantComponent {
        private Plant _plant;

        private float _overhead;
        private float _underwaterThreshold;
        private AnimationCurve _deteriorationCurve;
        private AnimationCurve _growthCurve;
        private const float INIT_WATER_LEVEL_MULTIPLIER = .5f;

        private float _waterCapacity;
        private float _hydrationRate;
        private float _waterLevel;

        public float WaterLevel => _waterLevel;
        public float WaterCapacity => _waterCapacity;
        public float GetNormalizedWaterLevel() => _waterLevel / _waterCapacity;

        // x = waterLevel / capacity; x=1 is ideal (at capacity), x=0 is empty, x=(1+overhead) is clamp ceiling
        private float X => _waterCapacity > 0 ? _waterLevel / _waterCapacity : 0f;
        public bool IsUnderwatered => X < _underwaterThreshold;
        public bool IsOverwatered => X > 1f;

        public void Initialize(Plant plant) {
            _plant = plant;
            var plantDef = plant.Definition;
            _overhead = plantDef.WateringOverhead;
            _underwaterThreshold = plantDef.UnderwaterThreshold;
            _deteriorationCurve = plantDef.DeteriorationCurve;
            _growthCurve = plantDef.GrowthCurve;

            Setup(plant.Lifecycle.InitialStage.WaterCapacity, plant.Stats.EffectiveHydrationRate);

            plant.Stats.OnStatsRecomputed += () => Setup(_waterCapacity, _plant.Stats.EffectiveHydrationRate);
            plant.Lifecycle.OnStageChanged += def => Setup(def.WaterCapacity, _plant.Stats.EffectiveHydrationRate);
            plant.OnTick += OnTick;
        }

        private void ChangeWaterLevel(float delta) {
            _waterLevel += delta;
            _waterLevel = Mathf.Clamp(_waterLevel, 0, _waterCapacity * (1 + _overhead));
        }

        public void Setup(float waterCapacity, float hydrationRate) {
            var newWaterLevelModifier = _waterCapacity != 0 ? GetNormalizedWaterLevel() : INIT_WATER_LEVEL_MULTIPLIER;
            _waterCapacity = waterCapacity;
            _hydrationRate = hydrationRate;
            _waterLevel = waterCapacity * newWaterLevelModifier;
        }

        private void OnTick(int tickNum) {
            ChangeWaterLevel(-1 * _hydrationRate);
            _plant.Stats.SetBundle("water", GetBundle());
        }

        public StatModifierBundle GetBundle() => new() {
            GrowthMultiplierDelta = _growthCurve.Evaluate(X) - 1f,
            DeteriorationRateMultiplierDelta = _deteriorationCurve.Evaluate(X) - 1f
        };

        public void AddWater(float amount) {
            amount = Mathf.Max(amount, 0);
            ChangeWaterLevel(amount);
        }
    }
}