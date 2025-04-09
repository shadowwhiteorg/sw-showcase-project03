using _Game.Interfaces;

namespace _Game.Core.Events
{
    public struct SpinStartedEvent : IGameEvent {}
    public struct ResetCompletedEvent : IGameEvent {}
    public struct SpinCompletedEvent : IGameEvent 
    {
        public int WinningNumber;
    }
    public struct SpinWheelEvent : IGameEvent { }
    public struct ExitRouletteEvent : IGameEvent { }
    public struct BallDroppedEvent : IGameEvent{}

    public struct TargetNumberSetEvent : IGameEvent
    {
        public int TargetNumber;
        public TargetNumberSetEvent(int targetNumber)
        {
            TargetNumber = targetNumber;
        }
    }
}