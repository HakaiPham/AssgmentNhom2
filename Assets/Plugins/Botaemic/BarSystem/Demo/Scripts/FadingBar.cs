using Botaemic.BarSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Botaemic.Example
{
    public class FadingBar : Bar
    {
        [SerializeField] private Color _foregroundColor = Color.green;
        [SerializeField] private Color _backgroundColor = Color.black;
        [SerializeField] private Color _fadingColor = Color.red;
        [SerializeField] private float _updateSpeedInSeconds = 0.1f;

        private Image _bar = null;
        private Image _fadingImage = null;
        private float _currentBackgroundValue = 1f;
        private Transform _text = null;
        private Text _barText = null;
        private bool _isInitialized = false;
        private Image _backgroundImage = null;
        private void Start()
        {
            _bar = transform.Find("Bar").GetComponent<Image>();
            _backgroundImage = transform.Find("Background").GetComponent<Image>();
            _fadingImage = transform.Find("FadingImage").GetComponent<Image>();
            
#if UNITY_EDITOR
            if (!_bar)
            {
                Debug.LogError($"{gameObject.name}: There is no transform found on this Bar!"); 
            }
            if (!_backgroundImage)
            {
                Debug.LogError($"{gameObject.name}: There is no background image found on this Bar!"); 
            }
            if (!_fadingImage)
            {
                Debug.LogError($"{gameObject.name}: There is no fadingImage found on this Bar!"); 
            }
#endif
            
            _bar.color = _foregroundColor;
            _backgroundImage.color = _backgroundColor;
            _fadingImage.color = _fadingColor;

            _text = transform.Find("Text"); // If there is a text object then find it.
            if (_text) { _barText = _text.GetComponent<Text>(); }
            
        }

        public override void Initialize(Stat newStat)
        {
            base.Initialize(newStat);
            _isInitialized = true;
            if (_barText != null) { _barText.text = FormatString(); }
        }

        protected override void UpdateBar()
        {
            _bar.fillAmount = _stat.ValuePercentage;
            if (_barText)
            {
                _barText.text = FormatString();
            }
        }

        private void LateUpdate()
        {
            if (!_isInitialized) { return; }
            
            if (_stat.CurrentValue <= Mathf.Epsilon)
            {
                _fadingImage.fillAmount = 0f;
                _bar.fillAmount = 0f;
                return;
            }

            if (_fadingImage.fillAmount > _bar.fillAmount)
            {
                _currentBackgroundValue -= Time.deltaTime * _updateSpeedInSeconds;
                _fadingImage.fillAmount = _currentBackgroundValue;
                return;
            }

            if (_fadingImage.fillAmount < _bar.fillAmount)
            {
                _currentBackgroundValue = _bar.fillAmount;
                _fadingImage.fillAmount = _bar.fillAmount;
                return;
            }
        }

        private string FormatString()
        {
            return _stat.CurrentValue.ToString("#.") + "/" + _stat.MaxValue.ToString("#.");
        }
    }
}