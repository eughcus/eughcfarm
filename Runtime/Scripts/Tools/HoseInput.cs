using System;
using Eughc.PlayerSystems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Eughc.Farm {
    public class HoseInput : ContextInputBase {
        public event Action<InputAction.CallbackContext> OnWaterStart;
        public event Action<InputAction.CallbackContext> OnWaterStop;

        [SerializeField] private InputActionReference water;
        public InputAction Water => water.action;

        private void OnWaterStarted(InputAction.CallbackContext ctx) => OnWaterStart?.Invoke(ctx);
        private void OnWaterCanceled(InputAction.CallbackContext ctx) => OnWaterStop?.Invoke(ctx);


        protected override void Subscribe() {
            water.action.started += OnWaterStarted;
            water.action.canceled += OnWaterCanceled;
        }

        protected override void Unsubscribe() {
            water.action.started -= OnWaterStarted;
            water.action.canceled -= OnWaterCanceled;
        }
    }
}