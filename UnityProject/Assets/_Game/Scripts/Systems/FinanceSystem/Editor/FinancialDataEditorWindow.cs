using UnityEngine;
using UnityEditor;

namespace _Game.Systems.FinanceSystem
{
#if UNITY_EDITOR
    public class FinancialDataEditorWindow : EditorWindow
    {
        private FinancialDataManager _dataManager;
        private float _newBalance;

        [MenuItem("Tools/Financial Data Editor")]
        public static void ShowWindow()
        {
            GetWindow<FinancialDataEditorWindow>("Financial Data Editor");
        }

        private void OnEnable()
        {
            _dataManager = new FinancialDataManager();
            _newBalance = _dataManager.Data.Balance;
        }

        private void OnGUI()
        {
            GUILayout.Label("Current Balance", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Balance:", _dataManager.Data.Balance.ToString("F2"));

            GUILayout.Space(10);
            GUILayout.Label("Adjust Balance", EditorStyles.boldLabel);
            _newBalance = EditorGUILayout.FloatField("New Balance", _newBalance);

            if (GUILayout.Button("Set New Balance"))
            {
                float delta = _newBalance - _dataManager.Data.Balance;
                _dataManager.UpdateBalance(delta, "Balance manually adjusted via Editor");
            }
            
            if (GUILayout.Button("Refresh Data"))
            {
                _dataManager.Load();
            }

            if (GUILayout.Button("Reset Financial Data"))
            {
                _dataManager.Reset();
            }

            GUILayout.Space(10);
            GUILayout.Label("Transaction Log", EditorStyles.boldLabel);
            if (_dataManager.Data.Transactions != null)
            {
                foreach (var transaction in _dataManager.Data.Transactions)
                {
                    GUILayout.Label(
                        $"{transaction.Timestamp}: {transaction.Description} ({transaction.Amount:+0.00;-0.00})");
                }
            }
        }
    }
#endif
}