using _Game.Core.Events;
using _Game.Systems.UISystem.Models;
using _Game.Systems.UISystem.Views;

namespace _Game.Systems.UISystem.Screens
{
    public class GameOverUIScreen : BaseUIScreen<GameOverModel, GameOverView>
    {
        protected override void RegisterEvents()
        {
            Subscribe<GameEndedEvent>(OnGameEnded);
        }

        private void OnGameEnded(GameEndedEvent evt)
        {
            _model.SetResult(evt.IsWin, evt.FinalBalance);
            Show();
        }
    }
}