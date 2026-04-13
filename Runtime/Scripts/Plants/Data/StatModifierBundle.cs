using System;

namespace Eughc.Farm {
    [Serializable]
    public struct StatModifierBundle {
        public float GrowthMultiplierDelta; // additive delta, e.g. +0.5 = +50%
        public float HealthMultiplierDelta; // additive delta
        public float DeteriorationRateMultiplierDelta; // additive delta
        public float HydrationRateMultiplierDelta; // additive delta
        public int YieldBonus; // flat, e.g. +7

        public bool IsEmpty() =>
            GrowthMultiplierDelta == 0 &&
            HealthMultiplierDelta == 0 &&
            DeteriorationRateMultiplierDelta == 0 &&
            HydrationRateMultiplierDelta == 0 &&
            YieldBonus == 0;

        public static StatModifierBundle operator +(StatModifierBundle a, StatModifierBundle b) => new() {
            GrowthMultiplierDelta = a.GrowthMultiplierDelta + b.GrowthMultiplierDelta,
            HealthMultiplierDelta = a.HealthMultiplierDelta + b.HealthMultiplierDelta,
            DeteriorationRateMultiplierDelta = a.DeteriorationRateMultiplierDelta + b.DeteriorationRateMultiplierDelta,
            HydrationRateMultiplierDelta = a.HydrationRateMultiplierDelta + b.HydrationRateMultiplierDelta,
            YieldBonus = a.YieldBonus + b.YieldBonus
        };
    }
}