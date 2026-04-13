using Eughc.BaseUI;

namespace Eughc.Farm.UI {
    public class UIPlantStatItem : IUIListItem {
        public string DisplayName { get; }
        public string Value { get; }

        public UIPlantStatItem(string label, string value) {
            DisplayName = label;
            Value = value;
        }
    }
}