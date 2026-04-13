using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eughc.Farm {
    public class PlantSystemFertilizer : MonoBehaviour, IPlantComponent {
        public event Action<int, StatModifierBundle> OnSlotChanged;

        private Plant _plant;
        private int _slotCount;
        public int SlotCount => _slotCount;

        private List<FertilizerPassiveBase> _activeFertilizers = new();
        public IReadOnlyList<FertilizerPassiveBase> ActiveFertilizers => _activeFertilizers;

        public void Initialize(Plant plant) {
            _plant = plant;
            _slotCount = plant.Definition.FertilizerSlots;

            plant.Lifecycle.OnStageChanged += def => StageChanged(def);
            plant.OnTick += OnTick;
        }

        public bool HasFreeSlot() => _activeFertilizers.Count < _slotCount;

        public bool TryApply(FertilizerPassiveBase fertilizer) {
            if (!HasFreeSlot()) return false;
            _activeFertilizers.Add(fertilizer);
            var slotNum = _activeFertilizers.IndexOf(fertilizer);
            _plant.Stats.SetBundle($"fert_{slotNum}", fertilizer.Modifier);
            _plant.DayNight?.RefreshCurrentPhase();
            OnSlotChanged?.Invoke(slotNum, fertilizer.Modifier);
            return true;
        }

        private void OnTick(int tickNum) {
            foreach (var fertilizer in _activeFertilizers)
                fertilizer.Tick(tickNum);
        }

        private void StageChanged(PlantStageDefinition definition) {
            foreach (var fertilizer in _activeFertilizers)
                fertilizer.StageChanged(definition.Name);
        }

        public FertilizerPassiveBase ExtractSlot(int index) => throw new NotImplementedException();
    }
}