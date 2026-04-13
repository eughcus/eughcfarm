using Eughc.PlayerSystems;
using Eughc.Quality;
using UnityEngine;

namespace Eughc.Farm {
    public abstract class FertilizerBase : EquipableBase, IUsable, IHasQuality {
        [SerializeField] private ItemQuality quality;
        public ItemQuality Quality => quality;
        public ItemQuality GetQuality() => quality;

        protected float QualityMultiplier => QualityRegistrySO.Instance.Get(quality).Weight;

        protected virtual bool CanFertilize(IInteractable target) {
            target.GameObject.TryGetComponent(out Plant plant);
            return plant != null;
        }

        protected abstract void Fertilize(Plant targetPlant);

        public bool CanUseOn(IInteractable target) => CanFertilize(target);

        public virtual void Use(IInteractable target) {
            target.GameObject.TryGetComponent(out Plant plant);
            if (plant == null) return;

            Fertilize(plant);
            Player.Instance.Hand.ConsumeItem();
        }
    }
}