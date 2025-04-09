using _Game.Core.Events;
using _Game.Interfaces;
using _Game.Systems.BetSystem;
using _Game.Systems.ChipSystem;
using _Game.Systems.UISystem.Models;
using _Game.Systems.UISystem.Views;
using UnityEngine;

namespace _Game.Systems.UISystem.Screens
{
    public class BetAreaUIScreen: BaseUIScreen<BetAreaUIModel, BetAreaUIView>
    {
        protected override void RegisterEvents()
        {

            _eventBus.Subscribe<GameInitializedEvent>(e => _view.Hide());
            _eventBus.Subscribe<ToTableEvent>(e => _view.Show());
            _eventBus.Subscribe<ChipDragEvent>(e=>OnChipDragEvent(e.ClosestArea,e.DraggedStack));
            _eventBus.Subscribe<ToRouletteEvent>(e => _view.Hide());
        }

        private void OnChipDragEvent(BetArea betArea, ChipStack stack)
        {
            _model.UpdateData(betArea.Data, stack);
        }
        
    }
}