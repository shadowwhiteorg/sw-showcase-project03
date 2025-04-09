using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace _Game.Systems.FinanceSystem
{
    public class FinancialDataManager
{
    private const string SaveFileName = "FinancialData.json";
    private const float DefaultBalance = 1000f;
    public FinancialData Data { get; private set; }

    public FinancialDataManager()
    {
        Debug.Log("[FinancialDataManager] Initialized.");
        Load();
    }

    public void Load()
    {
        string path = Path.Combine(Application.persistentDataPath, SaveFileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Data = JsonUtility.FromJson<FinancialData>(json);
            Debug.Log("[FinancialDataManager] Loaded data: " + json);
        }
        else
        {
            Data = new FinancialData 
            { 
                Balance = DefaultBalance, 
                Transactions = new List<TransactionEntry>() 
            };
            Debug.Log("[FinancialDataManager] No saved file found. Initializing with default data.");
            Save();
        }
    }

    /// <summary>
    /// Resets the financial data by setting the balance to default and clearing the transaction history.
    /// </summary>
    public void Reset()
    {
        Debug.Log("[FinancialDataManager] Resetting financial data.");
        Data = new FinancialData
        {
            Balance = DefaultBalance,
            Transactions = new List<TransactionEntry>()
        };
        Save();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(Data, true);
        string path = Path.Combine(Application.persistentDataPath, SaveFileName);
        File.WriteAllText(path, json);
        Debug.Log("[FinancialDataManager] Saved data: " + json);
    }

    /// <summary>
    /// Updates the balance and logs a transaction entry with the current timestamp.
    /// </summary>
    /// <param name="amount">The amount to add (or subtract if negative).</param>
    /// <param name="description">A description for the transaction.</param>
    public void UpdateBalance(float amount, string description)
    {
        Data.Balance += amount;
        TransactionEntry entry = new TransactionEntry
        {
            // Store the current time in ISO‑8601 (round-trip) format.
            Timestamp = DateTime.Now.ToString("o"),
            Description = description,
            Amount = amount
        };
        Data.Transactions.Add(entry);
        Save();
    }
}

}