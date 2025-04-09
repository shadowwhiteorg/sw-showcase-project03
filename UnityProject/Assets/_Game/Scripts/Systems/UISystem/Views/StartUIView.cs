using System;
using _Game.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Systems.UISystem.Views
{
    public class StartUIView : BaseUIView
    {
        [SerializeField] private Button startButton;

        public event Action OnStartClicked ;

        public override void Bind(IUIModel model)
        {
            base.Bind(model);
            startButton.onClick.AddListener(() => OnStartClicked?.Invoke());
        }
    }
}