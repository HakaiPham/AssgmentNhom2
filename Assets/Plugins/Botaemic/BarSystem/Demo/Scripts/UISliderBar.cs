using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Botaemic.Example
{
    [RequireComponent(typeof(Slider))]
    public class UISliderBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider = null;
        [SerializeField] private Text _text = null;

        private void Start()
        {
            _slider = GetComponent<Slider>();
            
        }

        private void Update()
        {
            float progress = Mathf.Abs(Mathf.Sin(Time.time));
            _slider.value = progress;
            _text.text = $"Loading... {(int) (progress * 100f)}%";
        }
    }
    
}