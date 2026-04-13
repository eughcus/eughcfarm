using System.Collections.Generic;
using System.Linq;
using Eughc.DayNight;
using UnityEngine;

namespace Eughc.Farm {
    public class PlantSystemDayNight : MonoBehaviour, IPlantComponent {
        private Plant _plant;
        private List<DayPhaseModEntry> _phaseModifiers;
        private IReadOnlyList<FertilizerPassiveBase> _fertilizers;
        private DayPhaseSO _currentPhase;
        private StatModifierBundle? _currentBundle;

        public PlantActiveEffect ActiveEffect {
            get {
                if (_currentBundle == null || _currentBundle.Value.IsEmpty()) return null;
                return new PlantActiveEffect(
                    _currentPhase?.Name ?? "Day Phase",
                    _currentPhase?.Icon,
                    _currentBundle.Value
                );
            }
        }

        public void Initialize(Plant plant) {
            _plant = plant;
            _phaseModifiers = plant.Definition.DayPhaseModifiers;
            _fertilizers = plant.Fertilizer.ActiveFertilizers;

            if (DayNightSystem.Instance == null) {
                enabled = false;
                return;
            }

            DayNightSystem.Instance.OnDayPhaseChanged += ApplyPhase;
            RefreshCurrentPhase();
        }

        private void OnDestroy() {
            if (DayNightSystem.Instance != null)
                DayNightSystem.Instance.OnDayPhaseChanged -= ApplyPhase;
        }

        public void RefreshCurrentPhase() {
            if (DayNightSystem.Instance != null)
                ApplyPhase(DayNightSystem.Instance.CurrentPhase);
        }

        private void ApplyPhase(DayPhaseSO phase) {
            _currentPhase = phase;

            var entry = _phaseModifiers.FirstOrDefault(e => e.Phase == phase);
            bool hasPlantEntry = entry.Phase != null;

            var sensitiveFertilizers = _fertilizers?
                .OfType<IDayPhaseSensitive>()
                .Select(f => f.GetDayPhaseModifier(phase))
                .Where(b => b.HasValue)
                .Select(b => b.Value)
                .ToList();

            bool hasSensitiveFertilizers = sensitiveFertilizers?.Count > 0;

            if (!hasPlantEntry && !hasSensitiveFertilizers) {
                _currentBundle = null;
                _plant.Stats.SetBundle("dayPhase", null);
                return;
            }

            var combined = hasPlantEntry ? entry.Bundle : default;
            if (hasSensitiveFertilizers)
                foreach (var f in sensitiveFertilizers)
                    combined += f;

            _currentBundle = combined;
            _plant.Stats.SetBundle("dayPhase", combined);
        }
    }
}