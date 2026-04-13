using System;
using UnityEngine;

namespace Eughc.Farm {
    public class PlantSystemHealth : MonoBehaviour, IPlantComponent {
        public event Action OnDead;
        public event Action OnRevived;
        public event Action<float> OnDamaged;

        private Plant _plant;

        private float _maxHealth;
        private float _currentHealth;

        public float GetMaxHealth() => _maxHealth;
        public float GetCurrentHealth() => _currentHealth;
        public float GetNormalizedHealth() => _currentHealth / _maxHealth;
        public bool IsDead => _currentHealth == 0;

        public void Initialize(Plant plant) {
            _plant = plant;
            _maxHealth = plant.Definition.BaseHealth;
            _currentHealth = _maxHealth * plant.Definition.InitialHealthFraction;

            plant.Stats.OnStatsRecomputed += Stats_OnStatsRecomputed;
            plant.OnTick += OnTick;
        }

        private void Stats_OnStatsRecomputed() {
            _maxHealth = _plant.Stats.EffectiveHealth;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        }

        private void OnTick(int tickNum) {
            Damage(_plant.Stats.EffectiveDeteriorationRate);
        }

        private void ChangeHealth(float amount) {
            _currentHealth += amount;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
            OnDamaged?.Invoke(_currentHealth);

            if (_currentHealth == 0)
                OnDead?.Invoke();
        }

        public void Revive() {
            _currentHealth = _maxHealth;
            OnRevived?.Invoke();
        }

        public void Damage(float amount) {
            amount = Mathf.Max(amount, 0);
            ChangeHealth(amount * -1);
        }

        public void Heal(float amount) {
            amount = Mathf.Max(amount, 0);
            ChangeHealth(amount);
        }
    }
}