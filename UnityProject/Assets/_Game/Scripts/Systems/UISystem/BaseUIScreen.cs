using System;
using System.Collections.Generic;
using System.Reflection;
using _Game.Interfaces;
using UnityEngine;

namespace _Game.Systems.UISystem
{
    public abstract class BaseUIScreen<TModel, TView> : MonoBehaviour
        where TModel : BaseUIModel
        where TView : BaseUIView
    {
        protected TModel _model;
        protected TView _view;
        protected IEventBus _eventBus;
    
        private readonly List<Action> _unsubscribeActions = new();

        public void Construct(TModel model, TView view, IEventBus eventBus)
        {
            _model = model;
            _view = view;
            _eventBus = eventBus;
        
            _view.Bind(_model);
            _view.Hide();
            RegisterEvents();
        }

        protected void Subscribe<T>(Action<T> handler) where T : IGameEvent
        {
            _eventBus.Subscribe(handler);
            _unsubscribeActions.Add(() => _eventBus.Unsubscribe(handler));
        }

        protected virtual void RegisterEvents() { }

        protected virtual void OnDestroy()
        {
            foreach (var unsubscribe in _unsubscribeActions)
                unsubscribe();
        
            _view.Hide();
        }

        public void Show() => _view.Show();
        public void Hide() => _view.Hide();
    }
}