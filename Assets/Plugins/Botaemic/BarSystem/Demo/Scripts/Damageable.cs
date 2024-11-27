using UnityEngine;

namespace Botaemic.Example
{
    [RequireComponent(typeof(Health))]
    public class Damageable : MonoBehaviour
    {
        [Tooltip("Multiplier to apply to the received damage")] [SerializeField]
        private float _damageMultiplier = 1f;

        [Tooltip("Multiplier to apply to self damage")] [SerializeField, Range(0, 1)]
        private float _sensibilityToSelfdamage = 0.5f;

        private Health _health = null;

        void Awake()
        {
            if (!TryGetComponent(out _health))
            {
                _health = GetComponentInParent<Health>();
            }
        }

        public void InflictDamage(float damage, GameObject damageSource = null)
        {
            if (_health)
            {
                float totalDamage = damage * _damageMultiplier;
                if (damageSource)
                {
                    if (_health.gameObject == damageSource)
                    {
                        totalDamage *= _sensibilityToSelfdamage;
                    }
                }

                _health.TakeDamage(totalDamage);
            }
        }
    }
}