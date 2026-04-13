namespace Eughc.Farm {
    public class YieldFertilizer : FertilizerPassiveBase {
        public override StatModifierBundle Modifier => new() {
            YieldBonus = (int)(QualityMultiplier * 10),
            HydrationRateMultiplierDelta = 0.5f
        };
    }
}