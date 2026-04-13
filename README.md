# Farm

Plant lifecycle, water, health, fertilizer, and harvest systems for farm-based games.

## Contents

- **Plant** — service-locator MonoBehaviour, owns tick relay and plant identity
- **Plant systems** — `PlantHealth`, `PlantWater`, `PlantLifecycle`, `PlantStats`, `PlantHarvest`, `PlantFertilizer`, `PlantDayNight` as composable MonoBehaviour components
- **Fertilizer system** — `FertilizerBase`, passive and active fertilizer base classes
- **Tools** — `Hose` (watering), `FruitBasket` (harvesting)
- **Data** — `PlantVariantSO`, `PlantStageDefinition`, `StatModifierBundle`, `PlantActiveEffect`
