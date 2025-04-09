using _Game.Core.Constants;
using _Game.Core.Events;
using _Game.Enums;
using _Game.Interfaces;
using UnityEngine;

namespace _Game.Core.Singletons
{
    public class StateManager : MonoBehaviour, IStateManager
    {
        [SerializeField] private Animator animator;
        
        private IEventBus _eventBus;
        public GameState GameState { get; private set; }

        public void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
            SubscribeToEvents();
        }

        private void SetStateStart()
        {
            GameState = GameState.Start;
            animator.SetTrigger(GameConstants.StartStateTrigger);
        }
        
        private void SetStateBet()
        {
            GameState = GameState.BetState;
            animator.SetTrigger(GameConstants.BetStateTrigger);
        }

        private void SetStateRoulette()
        {
            GameState = GameState.RouletteState;
            animator.SetTrigger(GameConstants.RouletteStateTrigger);
        }

        private void SetStateTable()
        {
            GameState = GameState.TableState;
            animator.SetTrigger(GameConstants.TableStateTrigger);
        }

        private void SubscribeToEvents()
        {
            _eventBus.Subscribe<GameStartedEvent>(e=>SetStateBet());
            _eventBus.Subscribe<ToBettingEvent>(e=>SetStateBet());
            _eventBus.Subscribe<ToTableEvent>(e=>SetStateTable());
            _eventBus.Subscribe<ToRouletteEvent>(e=>SetStateRoulette());
        }
    }
}