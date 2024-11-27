using UnityEngine;
using UnityEngine.UI;

namespace Botaemic.BarSystem
{
    public class SingleBarCanvas : Bar
    {
        [SerializeField] private Color _foregroundColor = Color.green;
        [SerializeField] private Color _backgroundColor = Color.black;

        private Image _bar = null;

        private void Start()
        {
            _bar = transform.Find("Bar").GetComponent<Image>();

#if UNITY_EDITOR
            if (!_bar)
            {
                Debug.LogError($"{gameObject.name}: There is no transform found on this Bar!"); 
            }
#endif
            
            _bar.color = _foregroundColor;
            transform.Find("Background").GetComponent<Image>().color = _backgroundColor;
        }
        
        protected override void UpdateBar() => _bar.fillAmount = _stat.ValuePercentage;
    }
}
