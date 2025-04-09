using _Game.Interfaces;

namespace _Game.Systems.RouletteSystem
{
    public abstract class BaseState
    {
        protected readonly StateMachine StateMachine;

        protected BaseState(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Tick(float deltaTime) { }
        public virtual void FixedTick(float fixedDeltaTime) { }
    }
}