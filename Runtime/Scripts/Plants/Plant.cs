using System;
using System.Collections.Generic;
using Eughc.Inventory;
using Eughc.Quality;
using UnityEngine;

namespace Eughc.Farm {
    public class Plant : MonoBehaviour {
        public event Action<int> OnFertilizerSlotChanged;
        public event Action<PlantStage> OnStageChanged;
        public event Action OnDead;
        public event Action<int> OnTick;

        [SerializeField] private PlantVariantSO defaultDefinition;
        [SerializeField] private PlantStage initialStage = PlantStage.Germinating;
        public PlantVariantSO Definition { get; set; }
        public ItemQuality Quality => Definition.Quality;

        [SerializeField] private Sprite icon;
        public Sprite Icon => icon;

        [SerializeField] private GameObject fruitPrefab;
        public GameObject FruitPrefab => fruitPrefab;

        [SerializeField] private string plantName = "Plant";
        public string Name => plantName;

        [SerializeField] private string plantDescription = "Basic plant";
        public string Description => plantDescription;

        private int _bornAt;
        private int _diedAt;

        // Components
        public PlantSystemStats Stats { get; private set; }
        public PlantSystemHealth Health { get; private set; }
        public PlantSystemWater Water { get; private set; }
        public PlantSystemLifecycle Lifecycle { get; private set; }
        public PlantSystemHarvest HarvestSystem { get; private set; }
        public PlantSystemFertilizer Fertilizer { get; private set; }
        public PlantSystemDayNight DayNight { get; private set; }

    #if UNITY_EDITOR
        private void OnValidate() {
            GetComponent<PlantVisual>()?.PreviewStage(initialStage, defaultDefinition);
        }
    #endif

        private void Awake() {
            Definition ??= defaultDefinition;

            Stats = GetComponent<PlantSystemStats>();
            Health = GetComponent<PlantSystemHealth>();
            Water = GetComponent<PlantSystemWater>();
            Lifecycle = GetComponent<PlantSystemLifecycle>();
            HarvestSystem = GetComponent<PlantSystemHarvest>();
            Fertilizer = GetComponent<PlantSystemFertilizer>();
            DayNight = GetComponent<PlantSystemDayNight>();
        }

        private void Start() {
            Stats.Initialize(this);
            Health.Initialize(this);
            Lifecycle.Initialize(this);
            Water.Initialize(this);
            HarvestSystem.Initialize(this);
            Fertilizer.Initialize(this);
            DayNight.Initialize(this);

            Health.OnDead += PlantHealth_OnDead;
            Lifecycle.OnStageChanged += def => OnStageChanged?.Invoke(def.Name);
            Fertilizer.OnSlotChanged += (slot, _) => OnFertilizerSlotChanged?.Invoke(slot);

            _bornAt = TickSystem.Instance.CurrentTickNum;
            TickSystem.Instance.OnTick += TickSystem_OnTick;

            Lifecycle.SetStage(initialStage);
        }

        private void TickSystem_OnTick(object sender, int tickNum) => OnTick?.Invoke(tickNum);

        private void PlantHealth_OnDead() {
            TickSystem.Instance.OnTick -= TickSystem_OnTick;
            _diedAt = TickSystem.Instance.CurrentTickNum;
            OnDead?.Invoke();
        }

        public int GetAge() {
            var age = Health.IsDead ? _diedAt - _bornAt : TickSystem.Instance.CurrentTickNum - _bornAt;
            return age;
        }

        public void Revive() {
            TickSystem.Instance.OnTick += TickSystem_OnTick;
            Health.Revive();
            Lifecycle.ResetStageProgress();
        }

        public IReadOnlyList<PlantActiveEffect> GetActiveEffects() {
            var effects = new List<PlantActiveEffect>();

            var stageEffect = Lifecycle.ActiveEffect;
            if (!stageEffect.Bundle.IsEmpty())
                effects.Add(stageEffect);

            if (Water.IsUnderwatered)
                effects.Add(new PlantActiveEffect("Underwatered", null, Water.GetBundle()));
            else if (Water.IsOverwatered)
                effects.Add(new PlantActiveEffect("Overwatered", null, Water.GetBundle()));

            var dayEffect = DayNight?.ActiveEffect;
            if (dayEffect != null)
                effects.Add(dayEffect);

            if (Fertilizer != null) {
                foreach (var fertilizer in Fertilizer.ActiveFertilizers) {
                    var bundle = fertilizer.Modifier;
                    if (bundle.IsEmpty()) continue;
                    fertilizer.TryGetComponent<InventoryItem>(out var item);
                    effects.Add(new PlantActiveEffect(
                        item?.DisplayName ?? fertilizer.name,
                        item?.Icon,
                        bundle
                    ));
                }
            }

            return effects;
        }

        [ContextMenu("[DEBUG] Log Current Stage")]
        private void DebugCurrentStage() => Debug.Log($"{Lifecycle.CurrentStage.Name}");
    }
}