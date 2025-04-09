using _Game.Systems.UISystem.Models;
using TMPro;
using UnityEngine;

namespace _Game.Systems.UISystem.Views
{
    public class GameOverView : BaseUIView
    {
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private TMP_Text _balanceText;

        protected override void OnModelUpdated()
        {
            base.OnModelUpdated();
            var model = (GameOverModel)_boundModel;
            _messageText.text = model.ResultMessage;
            _balanceText.text = $"Final Balance: ${model.FinalBalance}";
        }
    }
}