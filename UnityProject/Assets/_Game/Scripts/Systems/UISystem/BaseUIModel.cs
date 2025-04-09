using System;
using _Game.Interfaces;

namespace _Game.Systems.UISystem
{
    public abstract class BaseUIModel : IUIModel
    {
        public event Action OnUpdated;
        public bool IsActive { get; private set; }

        protected void NotifyUpdate()
        {
            OnUpdated?.Invoke();
        }

        public void SetActive(bool active)
        {
            IsActive = active;
            NotifyUpdate();
        }
    }
}