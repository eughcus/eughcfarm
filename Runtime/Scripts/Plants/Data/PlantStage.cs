namespace Eughc.Farm {
    public enum PlantStage {
        Germinating,   // just planted, nothing visible yet
        Seedling,      // first sprout
        Sapling,       // established, growing fast
        Young,         // slowing down, structure forming
        Mature,        // full size, starts deteriorating
        Harvestable,   // ready to pick
        Dead           // health = 0
    }
}