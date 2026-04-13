namespace Eughc.Farm {
    public class HealFertilizer : FertilizerActiveBase {
        protected override void Fertilize(Plant targetPlant) {
            targetPlant.Health.Heal(QualityMultiplier * 100f);
        }
    }
}