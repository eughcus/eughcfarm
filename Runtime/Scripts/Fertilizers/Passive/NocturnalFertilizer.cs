using Eughc.Quality;
using Eughc.DayNight;
using UnityEngine;

namespace Eughc.Farm {
    public class NocturnalFertilizer : FertilizerPassiveBase, IDayPhaseSensitive {
        [SerializeField] private DayPhaseSO[] nightPhases;

        public override StatModifierBundle Modifier => new();

        public StatModifierBundle? GetDayPhaseModifier(DayPhaseSO phase) {
            foreach (var nightPhase in nightPhases)
                if (nightPhase == phase)
                    return new StatModifierBundle {
                        GrowthMultiplierDelta = QualityMultiplier,
                        YieldBonus = (int)(QualityMultiplier * 5)
                    };

            return null;
        }
    }
}