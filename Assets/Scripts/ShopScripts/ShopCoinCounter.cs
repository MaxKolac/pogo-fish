using TMPro;
using UnityEngine;

public class ShopCoinCounter : MonoBehaviour, IDataPersistence
{
    [SerializeField] private TMP_Text coinText;

    public int CoinAmount { get; private set; }

    void Update()
    {
        coinText.text = CoinAmount.ToString();
    }

    public void AddCoins(int amount) => CoinAmount += amount;

    public void SpendCoins(int amount)
    {
        if (CanAfford(amount))
            CoinAmount -= amount;
    }

    public bool CanAfford(int amount) => amount <= this.CoinAmount;
    public void LoadData(GameData data) => CoinAmount = data.coinsAmount;
    public void SaveData(ref GameData data) => data.coinsAmount = CoinAmount;
}