using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Eughc.Farm {
    public class PlantSystemStats : MonoBehaviour, IPlantComponent {
        public event Action OnStatsRecomputed;

        private float _baseHealth;
        private float _effectiveHealth;
        public float EffectiveHealth => _effectiveHealth;

        private float _baseGrowthRate;
        private float _effectiveGrowthRate;
        public float EffectiveGrowthRate => _effectiveGrowthRate;

        private float _baseHydrationRate;
        private float _effectiveHydrationRate;
        public float EffectiveHydrationRate => _effectiveHydrationRate;

        private float _baseDeteriorationRate;
        private float _effectiveDeteriorationRate;
        public float EffectiveDeteriorationRate => _effectiveDeteriorationRate;

        private int _baseYield;
        private int _effectiveYield;
        public int EffectiveYield => _effectiveYield;

        private Dictionary<string, StatModifierBundle> _bundles = new();

        public void Initialize(Plant plant) {
            var def = plant.Definition;
            _baseHealth = def.BaseHealth;
            _baseGrowthRate = def.BaseGrowthRate;
            _baseHydrationRate = def.BaseHydrationRate;
            _baseDeteriorationRate = def.BaseDeteriorationRate;
            _baseYield = def.BaseYield;
            Recompute();
        }

        private void Recompute() {
            var healthMultiplier = 1 + _bundles.Values.Sum(b => b.HealthMultiplierDelta);
            var growthMultiplier = 1 + _bundles.Values.Sum(b => b.GrowthMultiplierDelta);
            var hydrationMultiplier = 1 + _bundles.Values.Sum(b => b.HydrationRateMultiplierDelta);
            var deteriorationMultiplier = 1 + _bundles.Values.Sum(b => b.DeteriorationRateMultiplierDelta);

            _effectiveHealth = Math.Max(0, _baseHealth * healthMultiplier);
            _effectiveGrowthRate = Math.Max(0, _baseGrowthRate * growthMultiplier);
            _effectiveHydrationRate = Math.Max(0, _baseHydrationRate * hydrationMultiplier);
            _effectiveDeteriorationRate = Math.Max(0, _baseDeteriorationRate * deteriorationMultiplier);
            _effectiveYield = Math.Max(0, _baseYield + _bundles.Values.Sum(b => b.YieldBonus));
        }

        public void SetBundle(string key, StatModifierBundle? bundle) {
            if (bundle.HasValue)
                _bundles[key] = bundle.Value;
            else
                _bundles.Remove(key);

            Recompute();
            OnStatsRecomputed?.Invoke();
        }
    }
}