using System;
using System.Collections.Generic;
using Eughc.PlayerSystems;
using UnityEngine;

namespace Eughc.Farm {
    public class FruitBasket : EquipableBase, IUsable {
        public event Action<Fruit> OnFruitAdded;

        [SerializeField] private int capacity;

        private List<Fruit> _contents = new();
        public IReadOnlyList<Fruit> Contents => _contents;

        public bool HasCapacity() {
            return _contents.Count < capacity;
        }

        public bool TryAdd(Fruit fruit) {
            if (!HasCapacity()) return false;
            _contents.Add(fruit);
            OnFruitAdded?.Invoke(fruit);
            return true;
        }

        public bool CanUseOn(IInteractable target) {
            target.GameObject.TryGetComponent(out Plant plant);
            return plant != null && plant.Lifecycle.CurrentStage.Name == PlantStage.Harvestable && plant.HarvestSystem.CanHarvest() && HasCapacity();
        }

        public void Use(IInteractable target) {
            target.GameObject.TryGetComponent(out Plant plant);
            if (plant == null) return;
            plant.HarvestSystem.Harvest(this);
        }
    }
}