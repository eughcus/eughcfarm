using Eughc.PlayerSystems;

namespace Eughc.Farm {
    public class ReviveFertilizer : FertilizerActiveBase {
        protected override bool CanFertilize(IInteractable target) {
            target.GameObject.TryGetComponent(out Plant plant);
            return plant != null && plant.Health.IsDead;
        }
        
        protected override void Fertilize(Plant plant) {
            plant.Revive();
        }
    }
}