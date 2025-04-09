using System;

namespace _Game.Interfaces
{
    public interface IUIModel
    {
        event Action OnUpdated;
        bool IsActive { get; }
        public void SetActive(bool active);
    }
}