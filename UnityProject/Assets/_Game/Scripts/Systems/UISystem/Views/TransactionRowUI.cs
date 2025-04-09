using System;
using TMPro;
using UnityEngine;

namespace _Game.Systems.UISystem.Views
{
    public class TransactionRowUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text timestampText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text amountText;

        public void Setup(DateTime timestamp, string description, float amount)
        {
            timestampText.text = timestamp.ToString("yyyy-MM-dd HH:mm");
            descriptionText.text = description;
            amountText.text = amount.ToString("+#.00;-#.00");
            amountText.color = amount >= 0 ? Color.green : Color.red;
        }
    }
}