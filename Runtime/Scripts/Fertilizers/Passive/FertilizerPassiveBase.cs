using Eughc.PlayerSystems;
using UnityEngine;

namespace Eughc.Farm {
    public abstract class FertilizerPassiveBase : FertilizerBase {
        public abstract StatModifierBundle Modifier { get; }

        public virtual void Tick(int tickNum) { }
        public virtual void StageChanged(PlantStage stage) { }

        protected override bool CanFertilize(IInteractable target) {
            target.GameObject.TryGetComponent(out Plant plant);
            return plant != null && plant.Fertilizer.HasFreeSlot();
        }

        protected override void Fertilize(Plant plant) => plant.Fertilizer.TryApply(this);

        public override void Use(IInteractable target) {
            target.GameObject.TryGetComponent(out Plant plant);
            if (plant == null) return;

            if (!plant.Fertilizer.TryApply(this)) return;

            Player.Instance.Hand.DropItem();
            transform.SetParent(plant.transform);
            StartCoroutine(DelayedDisable());
        }

        private System.Collections.IEnumerator DelayedDisable() {
            yield return new WaitForSeconds(.5f);
            gameObject.SetActive(false);
        }
    }
}