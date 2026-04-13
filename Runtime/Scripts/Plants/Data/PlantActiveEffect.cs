using UnityEngine;

namespace Eughc.Farm {
    public class PlantActiveEffect {
        public string Label { get; }
        public Sprite Icon { get; }
        public StatModifierBundle Bundle { get; }

        public PlantActiveEffect(string label, Sprite icon, StatModifierBundle bundle) {
            Label = label;
            Icon = icon;
            Bundle = bundle;
        }
    }
}