using Botaemic.BarSystem;
using UnityEngine;

namespace Botaemic.Example
{
    public class Actor : MonoBehaviour
    {
        [SerializeField] private float _maximumHealthpoints = 10f;
        [SerializeField] private float _maximumManapoints = 10f;
        [SerializeField] private float _manaRechargeRate = 1f;
        [SerializeField] private Bar _healthBar = null;
        [SerializeField] private Bar _manaBar = null;

        private Stat _health = null;
        private Stat _mana = null;

        private void Start()
        {
            _health = new Stat(_maximumHealthpoints);
            _mana = new Stat(_maximumManapoints);

            if (_healthBar != null) { _healthBar.Initialize(_health); }
            if (_manaBar != null) { _manaBar.Initialize(_mana); }
        }

        void Update()
        {
            if (_health.CurrentValue < Mathf.Epsilon)
            {
                Destroy(gameObject);
            }
            RechargeMana(_manaRechargeRate * Time.deltaTime);
        }

        private void RechargeMana(float amount)
        {
            _mana.AddPoints(amount);
        }

        public void InflictDamage(float amount)
        {
            _health.RemovePoints(amount);
        }

        public void Heal(float amount)
        {
            _health.AddPoints(amount);
        }

        public void DrainMana(float amount)
        {
            _mana.RemovePoints(amount);
        }
    }
}