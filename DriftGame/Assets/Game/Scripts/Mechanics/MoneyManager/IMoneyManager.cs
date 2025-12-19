using System;

public interface IMoneyManager 
{
    int Get(MoneyType type);
    void Add(int amount, MoneyType type);
    bool Spend(int amount, MoneyType type);

    event Action<int> OnCashChanged;
    event Action<int> OnGemsChanged;
}
