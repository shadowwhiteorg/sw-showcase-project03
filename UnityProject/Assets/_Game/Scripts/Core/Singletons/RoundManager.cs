using _Game.Core.Events;
using _Game.Interfaces;
using _Game.Systems.PayoutSystem;
using UnityEngine;

namespace _Game.Core.Singletons
{
    /// <summary>
    /// RoundManager orchestrates the lifecycle of a roulette round.
    /// It listens for events that mark the start of a new betting phase
    /// (such as GameStartedEvent or ToBettingEvent), clears any existing bets,
    /// and tracks the current round number.
    /// It also listens for spin triggers to mark the closure of betting.
    /// </summary>
    public class RoundManager : MonoBehaviour
    {
        private IEventBus _eventBus;
        private int _roundNumber = 0;

        // Reference to the ActiveBetsManager that collects bets.
        private ActiveBetsManager _activeBetsManager;

        /// <summary>
        /// Initializes the RoundManager with the event bus and ActiveBetsManager.
        /// </summary>
        /// <param name="eventBus">Central event bus instance.</param>
        /// <param name="activeBetsManager">ActiveBetsManager instance that holds placed bets.</param>
        public void Construct(IEventBus eventBus, ActiveBetsManager activeBetsManager)
        {
            _eventBus = eventBus;
            _activeBetsManager = activeBetsManager;
            SubscribeEvents();
        }

        /// <summary>
        /// Subscribes to Key events to coordinate round lifecycle.
        /// </summary>
        private void SubscribeEvents()
        {
            _eventBus.Subscribe<GameStartedEvent>(OnGameStarted);
            _eventBus.Subscribe<ToBettingEvent>(OnToBetting);
            _eventBus.Subscribe<SpinWheelEvent>(OnSpinWheel);
        }

        /// <summary>
        /// Handles GameStartedEvent: a new game round is starting.
        /// </summary>
        private void OnGameStarted(GameStartedEvent evt)
        {
            StartNewRound();
        }

        /// <summary>
        /// Handles ToBettingEvent: transitions to the betting phase.
        /// </summary>
        private void OnToBetting(ToBettingEvent evt)
        {
            StartNewRound();
        }

        /// <summary>
        /// Handles SpinWheelEvent: betting is now closed and the spin begins.
        /// </summary>
        private void OnSpinWheel(SpinWheelEvent evt)
        {
            Debug.Log($"RoundManager: Round {_roundNumber} is spinning. Betting is now closed.");
            // Optionally, fire an event to signal that betting is locked.
        }

        /// <summary>
        /// Starts a new round by incrementing the round counter and clearing active bets.
        /// It fires a BetsClearedEvent so that systems like ActiveBetsManager can reset their state.
        /// </summary>
        public void StartNewRound()
        {
            _roundNumber++;
            Debug.Log($"RoundManager: Starting new round #{_roundNumber}");
            // Fire BetsClearedEvent to clear any existing bets.
            _eventBus.Fire(new BetsClearedEvent(0));
        }

    }
}