using System.Linq;
using _Game.Interfaces;
using _Game.Systems.UISystem.Models;
using TMPro;
using UnityEngine;

namespace _Game.Systems.UISystem.Views
{
    public class BetAreaUIView : BaseUIView
    {
        [Header("References")]
        [SerializeField] private TMP_Text areaNumbersText;
        [SerializeField] private TMP_Text areaTypeText;
        [SerializeField] private TMP_Text payoutText;
        [SerializeField] private TMP_Text totalPayoutText;

        protected override void OnModelUpdated()
        {
            base.OnModelUpdated();
            var model = (BetAreaUIModel)_boundModel;
            areaNumbersText.text = model.Numbers != null ? string.Join("- ", model.Numbers.Select(n => n.ToString())) : "-";
            areaTypeText.text = $"{model.BetType.ToString()}" ;
            payoutText.text = $"Payout: {model.Payout.ToString()}";
            totalPayoutText.text = $"Total Payout: {model.TotalPayout.ToString()}";
            _canvasGroup.transform.position = model.BetAreaPosition;
        }
        
        public override void Bind(IUIModel model)
        {
            base.Bind(model);
        }
    }
}