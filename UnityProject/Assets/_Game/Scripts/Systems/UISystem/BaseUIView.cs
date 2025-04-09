using _Game.Enums;
using _Game.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Systems.UISystem
{
    public abstract class BaseUIView : MonoBehaviour, IUIView
    {
        [Header("Common UI Elements")]
        [SerializeField] protected Button closeButton;
        [SerializeField] protected CanvasGroup _canvasGroup;
        [SerializeField] protected TMP_Text _titleText;

        protected IUIModel _boundModel;

        public virtual void Bind(IUIModel model)
        {
            _boundModel = model;
        
            // if (closeButton != null)
            //     closeButton.onClick.AddListener(Hide);

            model.OnUpdated += OnModelUpdated;
            OnModelUpdated(); // Initial update
        }
        protected virtual void OnModelUpdated()
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = _boundModel.IsActive ? 1 : 0;
                _canvasGroup.blocksRaycasts = _boundModel.IsActive;
                _canvasGroup.interactable = _boundModel.IsActive;
            }
        }

        public virtual void Show() => _boundModel?.SetActive(true);
        public virtual void Hide() => _boundModel?.SetActive(false);

        protected virtual void OnDestroy()
        {
            if (_boundModel != null)
                _boundModel.OnUpdated -= OnModelUpdated;
        }
    }
}