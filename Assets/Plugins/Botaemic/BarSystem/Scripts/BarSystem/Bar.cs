using UnityEngine;

namespace Botaemic.BarSystem
{
    public abstract class Bar :  MonoBehaviour
    {
        protected Stat _stat = null;

        public virtual void Initialize(Stat newStat)
        {
            _stat = newStat;
            _stat.OnStatChanged += UpdateBar;
        }

        protected abstract void UpdateBar();
    }
}
