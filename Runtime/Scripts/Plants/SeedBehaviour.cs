using Eughc.PlayerSystems;
using Eughc.Quality;
using UnityEngine;

namespace Eughc.Farm {
    public class SeedBehaviour : PlaceableBase, IHasQuality {
        [SerializeField] private PlantVariantSO plantVariant;

        public ItemQuality GetQuality() => plantVariant.Quality;

        private void OnEnable() {
            Player.Instance.PlaceItems.OnPlaced += OnPlanted;
        }

        private void OnDisable() {
            Player.Instance.PlaceItems.OnPlaced -= OnPlanted;
        }

        private void OnPlanted(GameObject go) {
            if (!go.TryGetComponent<Plant>(out var plant)) return;
            plant.Definition = plantVariant;
        }
    }
}