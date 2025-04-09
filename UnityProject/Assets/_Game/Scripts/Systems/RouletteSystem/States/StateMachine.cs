using UnityEngine;

namespace _Game.Systems.RouletteSystem
{
    public class StateMachine
    {
        private BaseState _currentState;

        public void ChangeState(BaseState newState)
        {
            if (_currentState == newState)
                return;
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        public void Tick(float deltaTime)
        {
            _currentState?.Tick(deltaTime);
        }
        
        public void FixedTick(float fixedDeltaTime)
        {
            _currentState?.FixedTick(fixedDeltaTime);
        }

        public BaseState GetCurrentState()
        {
            return _currentState;
        }
    }
}