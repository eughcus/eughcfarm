using Eughc.BaseUI;
using TMPro;
using UnityEngine;

namespace Eughc.Farm.UI {
    public class UIPlantStatListElement : UIBasePickerListElement {
        [SerializeField] private TextMeshProUGUI valueLabel;

        public override void SetData(object data, string displayName) {
            base.SetData(data, displayName);  // sets nameLabel
            valueLabel.text = ((UIPlantStatItem)data).Value;
        }
    }
}