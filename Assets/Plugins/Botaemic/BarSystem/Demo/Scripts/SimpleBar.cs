using Botaemic.BarSystem;
using UnityEngine;

namespace Botaemic.Example
{
    public class SimpleBar : Bar
    {
        [SerializeField] private Color _foregroundColor = Color.green;
        [SerializeField] private Color _backgroundColor = Color.black;

        private Transform _bar = default(Transform);

        private void Start()
        {
            _bar = transform.Find("Bar");

#if UNITY_EDITOR
            if (!_bar)
            {
                Debug.LogError($"{gameObject.name}: There is no transform found on this Bar!");
            }
#endif

            _bar.transform.GetComponentInChildren<SpriteRenderer>().color = _foregroundColor;
            transform.Find("Background").GetComponentInChildren<SpriteRenderer>().color = _backgroundColor;
        }

        protected override void UpdateBar() => _bar.localScale = new Vector3(_stat.ValuePercentage, 1);
    }
}