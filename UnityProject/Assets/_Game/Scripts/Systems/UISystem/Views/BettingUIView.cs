using System;
using System.Collections;
using _Game.Interfaces;
using _Game.Systems.UISystem.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Systems.UISystem.Views
{
    public class BettingUIView : BaseUIView
    {
        [Header("References")]
        [SerializeField] private TMP_Text totalBetText;
        [SerializeField] private Button toTableButton;
        [SerializeField] private GameObject tutorialHand;

        [Header("Buttons")]
        [SerializeField] private Button increaseBetButton;
        [SerializeField] private Button decreaseBetButton;
        [SerializeField] private Button multiplierButton1;
        [SerializeField] private Button multiplierButton5;
        [SerializeField] private Button multiplierButton10;
        [SerializeField] private Button multiplierButton25;
        [SerializeField] private Button multiplierButton50;

        public event Action OnToTableClicked;
        public event Action OnIncreaseBet;
        public event Action OnDecreaseBet;
        public event Action<int> OnMultiplierButtonClicked;
        
        

        protected  override void OnModelUpdated()
        {
            base.OnModelUpdated();
        
            var model = (BettingUIModel)_boundModel;
            if(totalBetText)
                totalBetText.text = $"Total: {model.TotalBet}$";
            SetPlusButton(model.UnitBet);
            SetMinusButton(model.UnitBet);
            SetMultiplierButtons();
        }
        
        public void ResetViewValues()
        {
            var model = (BettingUIModel)_boundModel;
            if (totalBetText)
                totalBetText.text = $"Total: {model.TotalBet}$";
            SetPlusButton(model.UnitBet);
            SetMinusButton(model.UnitBet);
            SetMultiplierButtons();
        }
        
        public void SetMultiplierButtons()
        {
            var model = _boundModel as BettingUIModel;
            if(model.FinancialService == null) return;

            var currentBalance = model.FinancialService.GetBalance();
            if (multiplierButton1)
                multiplierButton1.interactable = currentBalance >= 1;
            if (multiplierButton5)
                multiplierButton5.interactable = currentBalance >= 5;
            if (multiplierButton10)
                multiplierButton10.interactable = currentBalance >= 10;
            if (multiplierButton25)
                multiplierButton25.interactable = currentBalance >= 25;
            if (multiplierButton50)
                multiplierButton50.interactable = currentBalance >= 50;
        }

        public void SetPlusButton(int unitBet)
        {
            var model = (BettingUIModel)_boundModel;
            if(model.FinancialService == null)return;
            increaseBetButton.interactable = model.FinancialService.GetBalance() >= unitBet;
            // Debug.Log("has balance " + (model.FinancialService.GetBalance() >= unitBet));
        }

        public void SetMinusButton(int unitBet){
            var model = (BettingUIModel)_boundModel;
            if(!model.ChipManager) return;
            decreaseBetButton.interactable = model.ChipManager.HasChipInStack(unitBet);
            // Debug.Log("has chip" + model.ChipManager.HasChipInStack(unitBet));
        }

        public override void Bind(IUIModel model)
        {
            base.Bind(model);
            
            if(increaseBetButton)
                increaseBetButton.onClick.AddListener(() => OnIncreaseBet?.Invoke());
            if(decreaseBetButton)
                decreaseBetButton.onClick.AddListener(() => OnDecreaseBet?.Invoke());
            if(toTableButton)
                toTableButton.onClick.AddListener(() => OnToTableClicked?.Invoke());
            if (multiplierButton1)
                multiplierButton1.onClick.AddListener(() => OnMultiplierButtonClicked?.Invoke(1));
            if (multiplierButton5)
                multiplierButton5.onClick.AddListener(() => OnMultiplierButtonClicked?.Invoke(5));
            if (multiplierButton10)
                multiplierButton10.onClick.AddListener(() => OnMultiplierButtonClicked?.Invoke(10));
            if (multiplierButton25)
                multiplierButton25.onClick.AddListener(() => OnMultiplierButtonClicked?.Invoke(25));
            if (multiplierButton50)
                multiplierButton50.onClick.AddListener(() => OnMultiplierButtonClicked?.Invoke(50));
                
        }

        private Coroutine _tutorialHandCoroutine;
        public void ShowTutorialHand()
        {
            if (tutorialHand == null || tutorialHand.gameObject.activeSelf || _tutorialHandCoroutine!=null) return;
            tutorialHand.SetActive(true);
            _tutorialHandCoroutine = StartCoroutine(AnimateTutorialHand());
        }

        public void HideTutorialHand()
        {
            if (tutorialHand == null) return;
            tutorialHand.SetActive(false);
            if (_tutorialHandCoroutine != null)
            {
                StopCoroutine(_tutorialHandCoroutine);
                _tutorialHandCoroutine = null;
            }
        }

        private IEnumerator AnimateTutorialHand()
        {
            while (true)
            {
                Vector3 initialScale = tutorialHand.transform.localScale;
                Vector3 scaledDown = initialScale * 0.8f;
                Vector3 initialPosition = tutorialHand.transform.localPosition;
                Vector3 movedUp = initialPosition + new Vector3(0, 50, 0);

                // Scale down
                float duration = 0.25f;
                float elapsedTime = 0;
                while (elapsedTime < duration)
                {
                    tutorialHand.transform.localScale = Vector3.Lerp(initialScale, scaledDown, elapsedTime / duration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                tutorialHand.transform.localScale = scaledDown;

                // Move up
                elapsedTime = 0;
                while (elapsedTime < duration)
                {
                    tutorialHand.transform.localPosition =
                        Vector3.Lerp(initialPosition, movedUp, elapsedTime / duration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                tutorialHand.transform.localPosition = movedUp;

                // Scale up
                elapsedTime = 0;
                while (elapsedTime < duration)
                {
                    tutorialHand.transform.localScale = Vector3.Lerp(scaledDown, initialScale, elapsedTime / duration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                tutorialHand.transform.localScale = initialScale;

                // Move back to initial position
                elapsedTime = 0;
                while (elapsedTime < duration)
                {
                    tutorialHand.transform.localPosition =
                        Vector3.Lerp(movedUp, initialPosition, elapsedTime / duration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                tutorialHand.transform.localPosition = initialPosition;
                
                yield return null;
            }
        }
        
        
    }
}