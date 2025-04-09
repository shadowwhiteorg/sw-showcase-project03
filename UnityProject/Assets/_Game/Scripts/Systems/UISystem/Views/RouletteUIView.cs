using System;
using System.Collections;
using _Game.Interfaces;
using _Game.Systems.UISystem.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Systems.UISystem.Views
{
    public class RouletteUIView : BaseUIView
    {
        [Header("References")]
        [SerializeField] private TMP_Text totalBetAmountText;
        [SerializeField] private TMP_Text totalPayoutText;
        [SerializeField] private GameObject winningParent;
        [SerializeField] private TMP_Text winningNumberText;
        [SerializeField] private Slider targetNumberSlider;
        [SerializeField] private TMP_Text targetNumberText;
        
        [Header("Buttons")]
        [SerializeField] private Button rollButton;
        [SerializeField] private Button toBettingButton;
        // [SerializeField] private Button toTableButton;
        
        public int CurrentTargetNumber { get; private set; }
        
        public event Action<int> OnTargetNumberChanged;
        public event Action OnRollButtonClicked; 
        public event Action OnToTableButtonClicked;
        public event Action OnToBettingButtonClicked;
        
        protected override void OnModelUpdated()
        {
            base.OnModelUpdated();
        
            var model = (RouletteUIModel)_boundModel;
            totalBetAmountText.text = $"{model.TotalBet}$";
            winningNumberText.text = $"{model.WinningNumber}";
            totalPayoutText.text = $"{model.TotalPayout}$";
        }

        public override void Bind(IUIModel model)
        {
            base.Bind(model);
            
            rollButton.onClick.AddListener(() => OnRollButtonClicked?.Invoke());
            toBettingButton.onClick.AddListener(() => OnToBettingButtonClicked?.Invoke());
            
            targetNumberSlider.minValue = 0;
            targetNumberSlider.maxValue = 36;

            targetNumberSlider.onValueChanged.AddListener(value =>
            {
                CurrentTargetNumber = Mathf.RoundToInt(value);
                targetNumberText.text = CurrentTargetNumber.ToString();
                OnTargetNumberChanged?.Invoke(CurrentTargetNumber);
            });
        }
        
        public void SetRandomTargetNumber()
        {
            int randomValue = UnityEngine.Random.Range(0, 37);
            targetNumberSlider.SetValueWithoutNotify(randomValue);
            targetNumberText.text = randomValue.ToString();
            CurrentTargetNumber = randomValue;
            OnTargetNumberChanged?.Invoke(randomValue);
        }

        
        
        public void EnableDisableRollButton(bool enable)
        {
            rollButton.interactable = enable;
        }

        public void EnableDisableToBettingButton(bool enable)
        {
            toBettingButton.interactable = enable;
        }
        
        public void ShowHideWinningNumber(bool show)
        {
            if (show)
            {
                StartCoroutine(ScaleUpWinningParent());
            }
            else
            {
                winningParent.transform.localScale = Vector3.zero;
                winningParent.SetActive(false);
            }
        }

        private IEnumerator ScaleUpWinningParent()
        {
            winningParent.SetActive(true);
            Transform parentTransform = winningParent.transform;
            parentTransform.localScale = Vector3.zero;

            float duration = 0.5f; // Duration of the scaling animation
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float scale = Mathf.Lerp(0f, 1f, elapsedTime / duration);
                parentTransform.localScale = new Vector3(scale, scale, scale);
                yield return null;
            }

            parentTransform.localScale = Vector3.one;
        }
        
        public void ShowHideTotalPayout(bool show)
        {
            if (show)
            {
                StartCoroutine(ScaleUpTotalPayout());
            }
            else
            {
                totalPayoutText.transform.localScale = Vector3.zero;
                totalPayoutText.gameObject.SetActive(false);
            }
        }
        
        private IEnumerator ScaleUpTotalPayout()
        {
            yield return new WaitForSeconds(0.5f);
            totalPayoutText.gameObject.SetActive(true);
            Transform parentTransform = totalPayoutText.transform;
            parentTransform.localScale = Vector3.zero;

            float duration = 0.5f; // Duration of the scaling animation
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float scale = Mathf.Lerp(0f, 1f, elapsedTime / duration);
                parentTransform.localScale = new Vector3(scale, scale, scale);
                yield return null;
            }

            parentTransform.localScale = Vector3.one;
        }
    }
}