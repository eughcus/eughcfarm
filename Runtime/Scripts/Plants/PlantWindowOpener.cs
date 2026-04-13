using System;
using Eughc.PlayerSystems;
using UnityEngine;

namespace Eughc.Farm {
    [RequireComponent(typeof(Plant))]
    public class PlantWindowOpener : InteractableBase {
        public static event Action<Plant> OnWindowRequested;

        private Plant _plant;

        public override bool IsActive => true;

        private void Start() {
            _plant = GetComponent<Plant>();
            
            _name = "Window Opener";
            _description = "Info";
            _interactionType = InteractionType.Secondary;

        }

        public override void Interact(Transform sender) {
            OnWindowRequested?.Invoke(_plant);
        }
    }
}