using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Core.Constants;
using _Game.Interfaces;
using _Game.Systems.FinanceSystem;
using _Game.Systems.UISystem.Models;
using _Game.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Systems.UISystem.Views
{
    public class FinanceUIView : BaseUIView
    {
        [Header("UI References")] [SerializeField]
        private TMP_Text balanceText;

        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Transform contentRoot;
        [SerializeField] private TMP_Text emptyText;
        [SerializeField] private TransactionRowUI transactionRowPrefab;
        [SerializeField] private Button openButton;
        [SerializeField] private RectTransform panelParent;


        private ObjectPool<TransactionRowUI> _transactionRowPool;
        private List<TransactionRowUI> _activeRows = new List<TransactionRowUI>();
        private float _panelWidth;
        private Coroutine _openCoroutine;
        private Coroutine _closeCoroutine;

        // Local events that external systems may subscribe to.
        public event System.Action OnOpenButtonClicked;
        public event System.Action OnCloseButtonClicked;

        public override void Bind(IUIModel model)
        {
            base.Bind(model);

            // Set up object pool for transaction rows.
            _transactionRowPool = new ObjectPool<TransactionRowUI>(
                transactionRowPrefab,
                initialSize: 20,
                parent: contentRoot
            );

            _panelWidth = GetComponent<RectTransform>().rect.width;
            panelParent.anchoredPosition = new Vector2(-_panelWidth, 0);

            // When the panel is opened, force the UI model to refresh its data.
            openButton.onClick.AddListener(() =>
            {
                OnOpenButtonClicked?.Invoke();
                (model as FinanceUIModel)?.RefreshData();
                ShowHidePanel(true);
            });

            // Close the panel.
            closeButton.onClick.AddListener(() =>
            {
                OnCloseButtonClicked?.Invoke();
                ShowHidePanel(false);
            });
        }

        protected override void OnModelUpdated()
        {
            base.OnModelUpdated();
            var financeModel = _boundModel as FinanceUIModel;
            if (balanceText != null)
            {
                balanceText.text = $"Balance: ${financeModel.CurrentBalance:0.00}";
            }

            Populate(financeModel.Transactions);
        }

        public void Populate(List<TransactionEntry> transactions)
        {
            ClearActiveRows();

            if (transactions == null || transactions.Count == 0)
            {
                emptyText.gameObject.SetActive(true);
                return;
            }

            emptyText.gameObject.SetActive(false);

            // For each transaction, get a row from the pool and set it up.
            foreach (var entry in transactions)
            {
                if(_transactionRowPool == null)return;
                var row = _transactionRowPool.Get();
                row.Setup(entry.DateTimeValue, entry.Description, entry.Amount);
                row.transform.SetParent(contentRoot, false);
                _activeRows.Add(row);
            }
        }

        private void ClearActiveRows()
        {
            foreach (var row in _activeRows)
            {
                _transactionRowPool.Return(row);
            }

            _activeRows.Clear();
        }

        public void ShowHidePanel(bool show)
        {
            closeButton.gameObject.SetActive(show);
            openButton.gameObject.SetActive(!show);

            if (show)
            {
                if (_closeCoroutine != null)
                    StopCoroutine(_closeCoroutine);
                _openCoroutine = StartCoroutine(SmoothMove(panelParent, panelParent.anchoredPosition, Vector2.zero,
                    GameConstants.FinancePanelMovementDuration));
            }
            else
            {
                if (_openCoroutine != null)
                    StopCoroutine(_openCoroutine);
                _closeCoroutine = StartCoroutine(SmoothMove(panelParent, panelParent.anchoredPosition,
                    new Vector2(-_panelWidth, 0), GameConstants.FinancePanelMovementDuration));
            }
        }

        private IEnumerator SmoothMove(RectTransform rectTransform, Vector2 from, Vector2 to, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                rectTransform.anchoredPosition = Vector2.Lerp(from, to, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            rectTransform.anchoredPosition = to;
        }
    }
}