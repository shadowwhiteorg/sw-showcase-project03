using _Game.Core.Events;
using _Game.Systems.UISystem.Models;
using _Game.Systems.UISystem.Views;
using UnityEngine;

namespace _Game.Systems.UISystem.Screens
{
    public class TableUIScreen : BaseUIScreen<TableUIModel, TableUIView>
    {
        protected override void RegisterEvents()
        {
            base.RegisterEvents();
            _view.OnToBetButtonClicked += OnToBet;
            _view.OnToRouletteButtonClicked += OnToRoulette;
            
            _eventBus.Subscribe<ToTableEvent>(e=>_view.Show());
            _eventBus.Subscribe<ToBettingEvent>(e=>_view.Hide());
            _eventBus.Subscribe<ToRouletteEvent>(e=>_view.Hide());
            _eventBus.Subscribe<TotalBetsCalculatedEvent>(e=>OnTotalBetCalculated(e.TotalBet));
        }

        private void OnToRoulette()
        {
            _eventBus.Fire(new ToRouletteEvent());
            Debug.Log("ToRoulette");
        }

        private void OnToBet()
        {
            _eventBus.Fire(new ToBettingEvent());
        }
        
        private void OnTotalBetCalculated(int totalBet)
        {
            _model.SetTotalBets(totalBet);
        }
    }
}