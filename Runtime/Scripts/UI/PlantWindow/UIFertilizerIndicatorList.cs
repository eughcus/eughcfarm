using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Eughc.Farm.UI {
    public class UIFertilizerIndicatorList : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI fertilizersLabel;
        [SerializeField] private GameObject uiSlotPrefab;

        private void Clear() {
            fertilizersLabel.enabled = false;

            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }
        }

        public void Setup(int slots, IReadOnlyList<FertilizerPassiveBase> fertilizers) {
            Clear();

            if (fertilizers == null) return;

            for (int slotIndex = 0; slotIndex < slots; slotIndex++) {
                var indicatorGameObject = Instantiate(uiSlotPrefab, transform);
                var indicator = indicatorGameObject.GetComponent<UIFertilizerIndicator>();

                if (fertilizers.Count > slotIndex) {
                    var fertilizer = fertilizers[slotIndex];
                    indicator.Setup(fertilizer);
                }
            }

            fertilizersLabel.enabled = true;
        }
    }
}