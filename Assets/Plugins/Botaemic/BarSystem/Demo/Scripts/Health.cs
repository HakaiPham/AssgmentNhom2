using Botaemic.BarSystem;
using UnityEngine;

namespace Botaemic.Example
{
    public class Health : MonoBehaviour
    {
        [Tooltip("Maximum amount of health")] [SerializeField]
        private float _maxHealth = 10f;

        [Tooltip("Health bar to display, can be empty")] [SerializeField]
        private Bar _healthBar = null;

        private Stat _health = null;

        void Start()
        {
            _health = new Stat(_maxHealth);
            if (_healthBar != null) { _healthBar.Initialize(_health); }
        }

        public void Heal(float amount)
        {
            _health.AddPoints(amount);
        }

        public void TakeDamage(float amount)
        {
            _health.RemovePoints(amount);

            if (_health.CurrentValue <= Mathf.Epsilon)
            {
                Destroy(gameObject);
            }
        }
    }
}