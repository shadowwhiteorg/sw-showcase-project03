using System;
using System.Collections.Generic;
using _Game.Interfaces;
using UnityEngine;

namespace _Game.Utils
{
    public class ManagerRunner : MonoBehaviour
    {
        private readonly List<IUpdatableManager> _managers = new();
        private bool _isPaused = false;

        public void Register(IUpdatableManager manager)
        {
            if (!_managers.Contains(manager))
                _managers.Add(manager);
        }

        public void Unregister(IUpdatableManager manager)
        {
            if (_managers.Contains(manager))
                _managers.Remove(manager);
        }

        public void SetPaused(bool paused)
        {
            _isPaused = paused;
        }

        private void Update()
        {
            if (_isPaused) return;

            float deltaTime = Time.deltaTime;
            foreach (var manager in _managers)
            {
                manager.Update(deltaTime);
            }
        }

        private void FixedUpdate()
        {
            if (_isPaused) return;

            float fixedDeltaTime = Time.fixedDeltaTime;
            foreach (var manager in _managers)
            {
                manager.FixedUpdate(fixedDeltaTime);
            }
        }
    }
}