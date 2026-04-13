using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Eughc.Farm.UI {
    public class UIActiveEffectItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject tooltip;
        [SerializeField] private TextMeshProUGUI tooltipNameLabel;
        [SerializeField] private TextMeshProUGUI tooltipStatsLabel;

        private void Awake() => tooltip.SetActive(false);

        public void Setup(PlantActiveEffect effect) {
            iconImage.sprite = effect.Icon;
            tooltipNameLabel.text = effect.Label;
            tooltipStatsLabel.text = FormatBundle(effect.Bundle);
        }

        public void OnPointerEnter(PointerEventData eventData) => tooltip.SetActive(true);
        public void OnPointerExit(PointerEventData eventData) => tooltip.SetActive(false);

        private static string FormatBundle(StatModifierBundle bundle) {
            var parts = new List<string>();
            if (bundle.GrowthMultiplierDelta != 0) parts.Add($"Growth {FormatDelta(bundle.GrowthMultiplierDelta)}");
            if (bundle.HealthMultiplierDelta != 0) parts.Add($"Health {FormatDelta(bundle.HealthMultiplierDelta)}");
            if (bundle.DeteriorationRateMultiplierDelta != 0) parts.Add($"Deterioration {FormatDelta(bundle.DeteriorationRateMultiplierDelta)}");
            if (bundle.HydrationRateMultiplierDelta != 0) parts.Add($"Hydration Rate {FormatDelta(bundle.HydrationRateMultiplierDelta)}");
            if (bundle.YieldBonus != 0) parts.Add($"Yield {(bundle.YieldBonus > 0 ? "+" : "")}{bundle.YieldBonus}");
            return string.Join("\n", parts);
        }

        private static string FormatDelta(float delta) => $"{(delta > 0 ? "+" : "")}{delta:P0}";
    }
}