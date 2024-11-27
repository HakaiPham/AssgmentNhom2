using Botaemic.BarSystem;
using UnityEngine;

namespace Botaemic.Example
{
    public class ColoredBar : Bar
    {
        [SerializeField] private Color _safeColor = Color.green;
        [SerializeField, Range(0, 1f)] private float _mediumTrigger = .6f;
        [SerializeField] private Color _mediumColor = Color.yellow;
        [SerializeField, Range(0, 1f)] private float _dangerTrigger = .3f;
        [SerializeField] private Color _dangerColor = Color.red;

        private Transform _bar = default(Transform);
        private SpriteRenderer _renderer = null;

        private void Start()
        {
            _bar = gameObject.transform.Find("Bar");

#if UNITY_EDITOR
            if (!_bar)
            {
                Debug.LogError($"{gameObject.name}: There is no transform found on this Bar!");
            }
#endif

            _renderer = _bar.transform.GetComponentInChildren<SpriteRenderer>();
            
#if UNITY_EDITOR
            if (!_renderer)
            {
                Debug.LogError($"{gameObject.name}: There is SpriteRenderer found on this Bar!");
            }
#endif
            _renderer.color = _safeColor;
        }

        protected override void UpdateBar()
        {
            if (_stat.ValuePercentage > _mediumTrigger)
            {
                _renderer.color = _safeColor;
            }
            else if (_stat.ValuePercentage > _dangerTrigger)
            {
                _renderer.color = _mediumColor;
            }
            else
            {
                _renderer.color = _dangerColor;
            }

            _bar.localScale = new Vector3(_stat.ValuePercentage, 1);
        }
    }
}