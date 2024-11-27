using UnityEngine;

namespace Botaemic.Example
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private float _damage = 1f;

        private Damageable[] _damageables;
        private Actor[] _actors;

        public void OnDamagePressed()
        {
            _damageables = FindObjectsOfType<Damageable>(); //for demo purpose only
            foreach (var damageable in _damageables)
            {
                damageable.InflictDamage(_damage);
            }

            _actors = FindObjectsOfType<Actor>(); //for demo purpose only
            foreach (var actor in _actors)
            {
                actor.InflictDamage(_damage);
            }
        }

        public void OnHealPressed()
        {
            // Taking the _damage variable as heal amount.
            float healAmount = _damage;
            _damageables = FindObjectsOfType<Damageable>(); //for demo purpose only
            foreach (var damageable in _damageables)
            {
                damageable.InflictDamage(-healAmount);
            }

            _actors = FindObjectsOfType<Actor>(); //for demo purpose only
            foreach (var actor in _actors)
            {
                actor.Heal(healAmount);
            }
        }

        public void OnManaUsePressed()
        {
            // Taking the _damage variable as mana cost.
            float manaCost = _damage;
            _actors = FindObjectsOfType<Actor>(); //for demo purpose only
            foreach (var actor in _actors)
            {
                actor.DrainMana(manaCost);
            }
        }
    }
}