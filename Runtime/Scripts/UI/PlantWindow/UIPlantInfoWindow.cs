using Eughc.BaseUI;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using Eughc.PlayerSystems;
using System;
using Eughc.Quality;

namespace Eughc.Farm.UI {
    [RequireComponent(typeof(BoxCollider))]
    public class UIPlantInfoWindow : UIBaseWindow {
        public override bool Persistent => true;
        [SerializeField] private Image plantQuality;
        [SerializeField] private Image plantIcon;
        [SerializeField] private TextMeshProUGUI nameLabel;
        [SerializeField] private TextMeshProUGUI descriptionLabel;
        [SerializeField] private UIPlantStatList statList;
        [SerializeField] private UIFertilizerIndicatorList fertilizersList;
        [SerializeField] private UIActiveEffectList activeEffectsList;

        private Plant _plant;

        public void Show(Plant plant) {
            if (_plant != null)
                _plant.OnFertilizerSlotChanged -= Plant_OnFertilizerSlotChanged;

            plantQuality.color = QualityRegistrySO.Instance.Get(plant.Quality).Color;
            plantIcon.sprite = plant.Icon;

            nameLabel.text = plant.Name;
            descriptionLabel.text = plant.Description;

            statList.DataSource = () => new List<UIPlantStatItem> {
                new("Age", $"{plant.GetAge():0}"),
                new("Health:", $"{plant.Health.GetCurrentHealth():0} / {plant.Health.GetMaxHealth():0} ({plant.Health.GetNormalizedHealth():P0})  Det: {plant.Stats.EffectiveDeteriorationRate:0.##}/tick"),
                new("Water:", $"{plant.Water.WaterLevel:0} / {plant.Water.WaterCapacity:0} ({plant.Water.GetNormalizedWaterLevel():P0})  Hyd: {plant.Stats.EffectiveHydrationRate:0.##}/tick"),
                new("Stage:", $"{plant.Lifecycle.CurrentStage.Name}: {plant.Lifecycle.Progress:0} / {plant.Lifecycle.CurrentStage.Duration:0} ({plant.Lifecycle.GetStageProgressNormalized():P0})  Growth: {plant.Stats.EffectiveGrowthRate:0.##}/tick"),
                new("Yield:", plant.Lifecycle.CurrentStage.Name == PlantStage.Harvestable
                    ? $"{plant.HarvestSystem.RemainingFruits} remaining / {plant.Stats.EffectiveYield}"
                    : $"{plant.Stats.EffectiveYield}"),
            }.Cast<object>().ToList();

            statList.Refresh();

            plant.OnFertilizerSlotChanged += Plant_OnFertilizerSlotChanged;
            _plant = plant;
            UIWindowManager.Instance.Open<UIPlantInfoWindow>();

            fertilizersList.Setup(plant.Definition.FertilizerSlots, plant.Fertilizer.ActiveFertilizers);
            activeEffectsList.Setup(plant.GetActiveEffects());
        }

        private void Plant_OnFertilizerSlotChanged(int _) {
            fertilizersList.Setup(_plant.Definition.FertilizerSlots, _plant.Fertilizer.ActiveFertilizers);
        }

        private void OnDestroy() {
            if (_plant != null)
                _plant.OnFertilizerSlotChanged -= Plant_OnFertilizerSlotChanged;
        }

        protected override void Start() {
            base.Start();
            PlantWindowOpener.OnWindowRequested += PlantWindowOpener_OnWindowRequested;
            Player.Instance.ContextManager.OnCancelAtBase += Close;

            // todo: decouple this shit later
            TickSystem.Instance.OnTick += TickSystem_OnTick;
        }

        private void TickSystem_OnTick(object sender, int e) {
            if (_plant == null) return;
            statList.Refresh();
            activeEffectsList.Setup(_plant.GetActiveEffects());
        }

        private void PlantWindowOpener_OnWindowRequested(Plant plant) {
            Show(plant);
        }
    }
}