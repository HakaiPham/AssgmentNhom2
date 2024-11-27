using System;
using UnityEngine;

namespace Botaemic.BarSystem
{
    public class Stat
    {
        public float ValuePercentage { get => (CurrentValue / MaxValue); }
        public float CurrentValue { get; private set; }
        public float MaxValue { get; private set; }

        public event Action OnStatChanged;

        public Stat(float maximumValue)
        {
            MaxValue = maximumValue;
            CurrentValue = maximumValue;
        }

        public void RemovePoints(float amount)
        {
            CurrentValue -= amount;
            CurrentValue = Mathf.Clamp(CurrentValue, 0f, MaxValue);
            TriggerActions();
        }

        public void AddPoints(float amount)
        {
            CurrentValue += amount;
            CurrentValue = Mathf.Clamp(CurrentValue, 0f, MaxValue);
            TriggerActions();  
     
        }

        private void TriggerActions()
        {
            OnStatChanged?.Invoke();
        }
    }
}
