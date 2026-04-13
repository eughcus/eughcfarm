using System.Linq;
using Eughc.PlayerSystems;
using UnityEngine;

namespace Eughc.Farm {
    [RequireComponent(typeof(Plant))]
    public class PlantVisual : MonoBehaviour {

        private Plant _plant;

        [Tooltip("must be parallel to lifecycle")]
        [SerializeField] private GameObject[] stageVisuals;
        [SerializeField] private GameObject deadVisual;
        [SerializeField] private Material outlineMaterial;

        private bool _isSelected = false;

        private void Start() {
            _plant = GetComponent<Plant>();

            // if (_plant.Lifecycle.Count != visuals.Length) {
            //     Debug.LogError("Visuals array and plantDef lifecycle are not parallel!");
            //     Destroy(this);
            //     return;
            // }

            PlantWindowOpener.OnWindowRequested += PlantWindowOpener_OnWindowRequested;

            // todo: reconsider approach. 
            Player.Instance.ContextManager.OnCancelAtBase += ContextManager_OnCancelAtBase;

            _plant.OnStageChanged += Plant_OnStateChanged;
            _plant.OnDead += Plant_OnDead;
        }

        private void ContextManager_OnCancelAtBase() {
            _isSelected = false;
            DisableOutline();
        }

        private void PlantWindowOpener_OnWindowRequested(Plant plant) {
            if (_isSelected) DisableOutline();
            _isSelected = plant == _plant;
            if (_isSelected) EnableOutline();
        }

        private void Plant_OnDead() {
            HideAll();
            deadVisual.SetActive(true);
            if (_isSelected) EnableOutline();
        }

        private void HideAll() {
            DisableOutline();
            foreach (var stageVisual in stageVisuals) {
                stageVisual.SetActive(false);
            }
            deadVisual.SetActive(false);
        }

        private int GetStageIndex(PlantStage stage) {
            for (var i = 0; i < _plant.Definition.LifecycleStages.Count; i++)
                if (_plant.Definition.LifecycleStages[i].Name == stage) return i;
            return -1;
        }

        private void Plant_OnStateChanged(PlantStage stage) {
            var index = GetStageIndex(stage);
            if (index < 0 || index >= stageVisuals.Length) return;
            HideAll();
            stageVisuals[index].SetActive(true);
            if (_isSelected) EnableOutline();
        }

        public void EnableOutline() {
            if (outlineMaterial == null) return;

            var meshRenderers = GetComponentsInChildren<Renderer>(includeInactive: true);
            foreach (var meshRenderer in meshRenderers) {
                var currentMats = meshRenderer.materials.ToList();
                currentMats.Add(new Material(outlineMaterial));
                meshRenderer.materials = currentMats.ToArray();
            }
        }

    #if UNITY_EDITOR
        public void PreviewStage(PlantStage stage, PlantVariantSO definition) {
            if (definition == null) return;
            var index = definition.LifecycleStages.FindIndex(s => s.Name == stage);
            if (index < 0 || index >= stageVisuals.Length) return;
            UnityEditor.EditorApplication.delayCall += () => {
                if (this == null) return;
                foreach (var v in stageVisuals) v.SetActive(false);
                stageVisuals[index].SetActive(true);
            };
        }
    #endif

        public void DisableOutline() {
            if (outlineMaterial == null) return;
            
            var meshRenderers = GetComponentsInChildren<Renderer>(includeInactive: true);
            foreach (var meshRenderer in meshRenderers) {
                var currentMats = meshRenderer.materials.ToList();
                currentMats.RemoveAll(m => m.name.Contains(outlineMaterial.name));
                meshRenderer.materials = currentMats.ToArray();
            }
        }
    }
}