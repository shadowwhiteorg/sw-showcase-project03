using _Game.Interfaces;

namespace _Game.Systems.RouletteSystem
{
    public interface IRouletteState
    {
        void Enter();                     // Called once on entering the state
        void Exit();                      // Called once on leaving the state
        void Update(float deltaTime);     // Called every frame while active
        void HandleEvent(IGameEvent e);   // Handle events directly in states -- yessirrrrr!
    }
}