# Farm — Design Document

> Package: `com.eughc.farm`
> Plane project: PACKS — module: eughcfarm
> Namespace: `Eughc.Farm`

## Overview

A composable farm system. `Plant` is a service-locator MonoBehaviour; each subsystem is an independent `MonoBehaviour` implementing `IPlantComponent`. Plant behaviour emerges from the composition of components on a prefab.

## Package Structure

```
Runtime/
  Scripts/
    Plants/         Plant, IPlantComponent, PlantVisual, SeedBehaviour
    Plants/Systems/ PlantHealth, PlantWater, PlantLifecycle, PlantStats,
                    PlantHarvest, PlantFertilizer, PlantDayNight
    Plants/Data/    PlantVariantSO, PlantStageDefinition, PlantStage,
                    StatModifierBundle, PlantActiveEffect, DayPhaseModEntry
    Fertilizers/    FertilizerBase, FertilizerPassiveBase, FertilizerActiveBase,
                    IDayPhaseSensitive
    Tools/          Hose, HoseInput, HoseVisual, FruitBasket, FruitBasketVisual
    Fruit/          Fruit
    UI/             UIPlantInfoWindow and supporting list/item components
Documentation~/
  DESIGN.md
  DECISIONS.md
```

## Core Concepts

### Plant as Service Locator

`Plant` does not contain domain logic. It holds references to all subsystem components and exposes them as public properties. Consumers navigate via `plant.Health`, `plant.Water`, etc.

### IPlantComponent

All subsystems implement `IPlantComponent`:

```csharp
public interface IPlantComponent {
    void Initialize(Plant plant);
}
```

`Plant.Start()` calls `Initialize` on each component in a fixed order (Stats first, then Health, Lifecycle, Water, Harvest, Fertilizer, DayNight).

### Tick relay

`Plant` owns the `TickSystem` subscription and fires `event Action<int> OnTick`. Subsystems subscribe to `plant.OnTick` — none subscribe to `TickSystem` directly.

### StatModifierBundle

Additive modifier struct. Components produce bundles (water level effects, stage bonuses, fertilizer effects, day phase); `PlantStats` accumulates them via `SetBundle(key, bundle)` and recomputes effective values on change.

## Dependencies

- `com.eughc.playersystems` — `EquipableBase`, `IInteractable`, `Player`
- `com.eughc.daynight` — `DayNightSystem`, day phase callbacks
- `com.eughc.quality` — `ItemQuality`, `QualityRegistrySO`
- `com.eughc.baseui` — `UIBaseWindow`, `UIWindowManager`
- `com.eughc.inventory` — `InventoryItem`
