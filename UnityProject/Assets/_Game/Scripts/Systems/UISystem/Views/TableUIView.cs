using System;
using _Game.Interfaces;
using _Game.Systems.UISystem.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Systems.UISystem.Views
{
    public class TableUIView : BaseUIView
    {
        [Header("References")]
        [SerializeField] private TMP_Text totalBetsText;
        [SerializeField] private TMP_Text totalPayoutsText;

        [Header("Buttons")] 
        [SerializeField] private Button toBetAreaButton;
        [SerializeField] private Button toRouletteButton;

        public event Action OnToBetButtonClicked;
        public event Action OnToRouletteButtonClicked;

        protected override void OnModelUpdated()
        {
            base.OnModelUpdated();

            var model = (TableUIModel)_boundModel;
            totalBetsText.text = model.TotalBets.ToString();
            totalPayoutsText.text = model.TotalPayouts.ToString();
        }

        public override void Bind(IUIModel model)
        {
            base.Bind(model);
            
            toBetAreaButton.onClick.AddListener(() => OnToBetButtonClicked?.Invoke());
            toRouletteButton.onClick.AddListener(() => OnToRouletteButtonClicked?.Invoke());
        }
    }
}