# Farm — Decision Log

## Components over plain C# classes

Plain C# subsystems were converted to MonoBehaviour components so Unity's serialization, inspector workflow, and lifecycle hooks apply uniformly. PlantBehaviour became a service locator (`Plant`) rather than an orchestrator.

## PlantSystem prefix dropped; Eughc.Farm namespace added

`PlantSystemHealth` → `PlantHealth` etc. The `PlantSystem` prefix was redundant given the `Eughc.Farm` namespace. The `Plant` prefix is kept on types that would otherwise collide with common names (`Health`, `Water`) when consumed without a `using` alias.

## Tick relay via Plant.OnTick

Subsystems do not subscribe to `TickSystem` directly. `Plant` owns the subscription and relays via `event Action<int> OnTick`. This keeps TickSystem coupling to one place and makes it easy to pause/resume ticking on death/revive.

## AnimationCurve for water effects

Water's growth and deterioration influence is expressed as AnimationCurves (x = normalised water level) rather than linear thresholds. Gives designers asymmetric control without code changes.

## WateringOverhead serialized on PlantVariantSO

The ceiling for water level is `capacity × (1 + overhead)` where overhead defaults to 0.25. Serialized per-variant for balancing without touching code.
