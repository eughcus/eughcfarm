using System;
using System.Collections.Generic;
using System.Linq;

namespace Eughc.Farm {
    public static class PlantStageFSM {
        private static readonly Dictionary<PlantStage,PlantStage> _specialTransitions = new() {
            { PlantStage.Harvestable, PlantStage.Mature },
            // { PlantStage.Mature, PlantStage.Dead },
        };

        // handle automatic transitioning
        public static PlantStage? GetNextStage(PlantStage current, List<PlantStageDefinition> plantStageDefinitions) {
            // dead means dead
            if (current == PlantStage.Dead) return null; 

            if (_specialTransitions.TryGetValue(current, out var special))
                return special;

            bool foundCurrent = false;
            foreach (PlantStage stage in Enum.GetValues(typeof(PlantStage))) {
                if (stage == PlantStage.Dead) continue;

                if (foundCurrent && plantStageDefinitions.Any(s => s.Name == stage))
                    return stage;

                if (stage == current) foundCurrent = true;
            }
            return null;
        }
    }
}