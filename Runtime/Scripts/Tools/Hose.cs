using System;
using Eughc.PlayerSystems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Eughc.Farm {
    [RequireComponent(typeof(HoseInput))]
    public class Hose : EquipableBase, IActivatable {
        public bool ShowTooltip => true;
        public bool CanActivate => true;

        private HoseInput _hoseContextInput;
        public ContextInputBase ContextInput => _hoseContextInput;

        [SerializeField] private PlayerContextSO _hoseContext;
        public PlayerContextSO Context => _hoseContext;

        [SerializeField] private LayerMask hitLayerMask;
        [SerializeField] private float maxAngle = 60f;
        [SerializeField] private float maxRange = 10f;
        [SerializeField] private float flowRate = 1f;

        private HoseVisual _hoseVisual;
        private bool _isWatering;
        private Vector3 _impactPoint;
        private Camera _camera;
        private ControlMap _waterControlMap;

        private void Start() {
            _camera = Camera.main;
            _hoseVisual = GetComponent<HoseVisual>();
            
            _hoseContextInput = GetComponent<HoseInput>();
            _hoseContextInput.OnWaterStart += Input_OnWaterStart;
            _hoseContextInput.OnWaterStop += Input_OnWaterStop;
        }

        private void OnTransformParentChanged() {
            if (transform.parent == null) {
                if (_waterControlMap != null)
                    Player.Instance.ControlsManager.Unregister(_waterControlMap);
                return;
            }

            _hoseVisual?.OnEquip();

            if (_waterControlMap == null)
                _waterControlMap = new ControlMap(_hoseContextInput.Water, "Water", priority: 100);
            Player.Instance.ControlsManager.Register(_waterControlMap);
        }

        private void OnDestroy() {
            if (_waterControlMap != null)
                    Player.Instance.ControlsManager.Unregister(_waterControlMap);
            
            _hoseContextInput.OnWaterStart -= Input_OnWaterStart;
            _hoseContextInput.OnWaterStop -= Input_OnWaterStop;
        }

        private void Update() {
            if (!_isWatering) return;

            var hit = Player.Instance.RaycastProvider.GetHitUnderCrosshair(hitLayerMask, maxRange);
            Vector3 candidate = hit.HasValue
                ? hit.Value.point
                : _hoseVisual.NozzleTip.position + _camera.transform.forward * maxRange;

            Vector3 candidateDir = (candidate - _hoseVisual.NozzleTip.position).normalized;
            if (Vector3.Angle(_camera.transform.forward, candidateDir) <= maxAngle)
                _impactPoint = candidate;

            if (hit.HasValue && hit.Value.collider.TryGetComponent<Plant>(out var plant))
                plant.Water.AddWater(flowRate * Time.deltaTime);
        }

        private void LateUpdate() {
            if (!_isWatering) return;
            _hoseVisual.UpdateFlow(_impactPoint);
        }

        private void Input_OnWaterStart(InputAction.CallbackContext context) {
            _impactPoint = _hoseVisual.NozzleTip.position + _camera.transform.forward * maxRange;
            _isWatering = true;
            _hoseVisual.StartFlow();
        }

        private void Input_OnWaterStop(InputAction.CallbackContext context) {
            _isWatering = false;
            _hoseVisual.StopFlow();
        }

        public void Activate() {
            throw new NotImplementedException();
        }
    }
}