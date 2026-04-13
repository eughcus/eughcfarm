using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Eughc.Farm.UI {
    public class UIActiveEffectList : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private GameObject itemPrefab;

        public void Setup(IReadOnlyList<PlantActiveEffect> effects) {
            if (label != null)
                label.enabled = effects.Count > 0;

            if (transform.childCount != effects.Count) {
                foreach (Transform child in transform)
                    Destroy(child.gameObject);
                foreach (var effect in effects) {
                    var go = Instantiate(itemPrefab, transform);
                    go.GetComponent<UIActiveEffectItem>().Setup(effect);
                }
            } else {
                var items = GetComponentsInChildren<UIActiveEffectItem>();
                for (var i = 0; i < items.Length; i++)
                    items[i].Setup(effects[i]);
            }
        }
    }
}