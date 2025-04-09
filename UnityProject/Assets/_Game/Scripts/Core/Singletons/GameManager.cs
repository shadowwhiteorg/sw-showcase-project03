using _Game.Core.Events;
using _Game.Interfaces;
using UnityEngine;

namespace _Game.Core.Singletons
{
    public class GameManager
    {
        private IEventBus _eventBus;
        
        public GameManager(IEventBus eventBus)
        {
            _eventBus = eventBus;
            // Set target fps to 60
            SubscribeToEvents();
        }
        
        private void SubscribeToEvents()
        {
            _eventBus.Subscribe<GameStartedEvent>(OnGameStarted);
        }

        private void OnGameStarted(GameStartedEvent evt)
        {   
            // Debug.Log(evt.Message);
        }

    }
}