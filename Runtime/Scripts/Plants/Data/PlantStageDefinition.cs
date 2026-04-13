using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eughc.Farm {
    [Serializable]
    public class PlantStageDefinition {
        public PlantStage Name = PlantStage.Germinating;
        
        [Tooltip("Phase duration")]
        public float Duration = 10f;
        
        [Tooltip("How much water is healthy")]
        public float WaterCapacity = 100f;

        public float HydrationRateMultiplier = 1f;
        
        public float DeteriorationRateMultiplier = 1f;

        public StatModifierBundle GetStatModifierBundle() => new() {
            // convert from multiplier to delta
            HydrationRateMultiplierDelta = HydrationRateMultiplier - 1f,
            DeteriorationRateMultiplierDelta = DeteriorationRateMultiplier - 1f,
        };

        public static readonly List<PlantStageDefinition> DefaultLifecycle = new() {
            new PlantStageDefinition {
                Name = PlantStage.Germinating,
                Duration = 10,
                WaterCapacity = 100f,
                HydrationRateMultiplier = 1f,
                DeteriorationRateMultiplier = 0f
            },
            new PlantStageDefinition {
                Name = PlantStage.Sapling,
                Duration = 25,
                WaterCapacity = 100f,
                HydrationRateMultiplier = 1f,
                DeteriorationRateMultiplier = 0f
            },
            new PlantStageDefinition {
                Name = PlantStage.Young,
                Duration = 50,
                WaterCapacity = 100f,
                HydrationRateMultiplier = 1f,
                DeteriorationRateMultiplier = 0f
            },
            new PlantStageDefinition {
                Name = PlantStage.Mature,
                Duration = 100,
                WaterCapacity = 100f,
                HydrationRateMultiplier = 1f,
                DeteriorationRateMultiplier = 0f
            },
            new PlantStageDefinition {
                Name = PlantStage.Harvestable,
                Duration = 0,
                WaterCapacity = 100f,
                HydrationRateMultiplier = 1f,
                DeteriorationRateMultiplier = 0f
            },
        };
    }
}