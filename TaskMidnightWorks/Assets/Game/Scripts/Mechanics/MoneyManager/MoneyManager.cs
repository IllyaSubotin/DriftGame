using System;
using UnityEngine;

public class MoneyManager : IMoneyManager
{
    private MoneyData _data;

    public event Action<int> OnCashChanged;
    public event Action<int> OnGemsChanged;

    public MoneyManager(MoneyData data)
    {
        _data = data;
    }

    public int Get(MoneyType type)
    {
        return type switch
        {
            MoneyType.Cash => _data.Cash,
            MoneyType.Gems => _data.Gems,
            _ => 0
        };
    }

    public void Add(int amount, MoneyType type)
    {
        switch (type)
        {
            case MoneyType.Cash:
                _data.Cash += amount;
                OnCashChanged?.Invoke(_data.Cash);
                break;

            case MoneyType.Gems:
                _data.Gems += amount;
                OnGemsChanged?.Invoke(_data.Gems);
                break;
        }
    }

    public bool Spend(int amount, MoneyType type)
    {
        if (Get(type) < amount)
            return false;

        Add(-amount, type);
        return true;
    }
}

[Serializable]
public class MoneyData
{
    public int Cash;
    public int Gems;
}

public enum MoneyType
{
    Cash,
    Gems
}
