using UnityEngine;
using UnityEngine.UI;

namespace Botaemic.BarSystem
{
    [RequireComponent(typeof(Image))]
    public class SimpleImageBar : Bar
    {
        [SerializeField] protected Image _bar = null;

        private void Start()
        {
            if (!TryGetComponent(out _bar))
            {
                Debug.LogError($"There is no image assigned to this Bar on {gameObject.name}"); 
            }
        }

        protected override void UpdateBar() => _bar.fillAmount = _stat.ValuePercentage;
    }
}
