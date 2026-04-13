using System.Collections.Generic;
using Eughc.DayNight;
using Eughc.Quality;
using UnityEngine;

namespace Eughc.Farm {
    [CreateAssetMenu(fileName = "Quality", menuName = "Farm/PlantQuality")]
    public class PlantVariantSO : ScriptableObject {
        public float BaseHealth = 100f;

        [Tooltip("% of basehealth")]
        public float InitialHealthFraction = 0.5f;

        [Tooltip("Growth rate per tick")]
        public float BaseGrowthRate = 1f;

        [Tooltip("Water Level Deduction per tick")]
        public float BaseHydrationRate = 1f;

        [Tooltip("Health deterioration per tick")]
        public float BaseDeteriorationRate = 5f;

        [Tooltip("Qty of products per harvest")]
        public int BaseYield = 10;

        [Tooltip("Passive Fertilizer slots")]
        public int FertilizerSlots = 0;

        public ItemQuality Quality;

        public List<PlantStageDefinition> LifecycleStages = new(PlantStageDefinition.DefaultLifecycle);

        public List<DayPhaseModEntry> DayPhaseModifiers = new();

        [Header("Water Curves")]
        [Tooltip("Max water = capacity × (1 + overhead). 0.25 = 125% capacity ceiling.")]
        public float WateringOverhead = 0.25f;

        [Tooltip("Water level fraction below which the plant is considered underwatered. 0.25 = below 25% capacity.")]
        public float UnderwaterThreshold = 0.25f;

        [Tooltip("Deterioration rate by water level. x=0 empty, x=1 at capacity, x=(1+overhead) ceiling.")]
        public AnimationCurve DeteriorationCurve = new();

        [Tooltip("Growth rate by water level. x=0 empty, x=1 at capacity, x=(1+overhead) ceiling.")]
        public AnimationCurve GrowthCurve = new();
    }
}