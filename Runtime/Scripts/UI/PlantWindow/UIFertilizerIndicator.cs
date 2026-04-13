using Eughc.BaseUI.Integrations.Inventory;
using Eughc.Inventory;
using Eughc.Quality;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Eughc.Farm.UI {
    public class UIFertilizerIndicator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private Image qualityImage;
        [SerializeField] private Image fertilizerImage;

        [SerializeField] private GameObject tooltip;
        [SerializeField] private TextMeshProUGUI tooltipNameLabel;
        [SerializeField] private TextMeshProUGUI tooltipDescriptionLabel;

        private FertilizerPassiveBase _fertilizer;

        private void Awake() {
            tooltip.SetActive(false);
            qualityImage.enabled = false;
            fertilizerImage.enabled = false;
        }

        public void Setup(FertilizerPassiveBase fertilizer) {
            _fertilizer = fertilizer;

            qualityImage.color = QualityRegistrySO.Instance.Get(_fertilizer.Quality).Color;
            qualityImage.enabled = true;

            fertilizer.TryGetComponent(out InventoryItem inventoryItem);
            if (inventoryItem == null) return;

            fertilizerImage.sprite = inventoryItem.Icon;
            fertilizerImage.enabled = true;

            tooltipNameLabel.text = inventoryItem.DisplayName;
            tooltipDescriptionLabel.text = inventoryItem.Description;
        }

        public void Clear() {
            _fertilizer = null;

            qualityImage.color = QualityRegistrySO.Instance.Get(ItemQuality.Common).Color;
            qualityImage.enabled = false;

            fertilizerImage.sprite = null;
            fertilizerImage.enabled = false;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if (_fertilizer != null)
                tooltip.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData) {
            tooltip.SetActive(false);
        }
    }
}