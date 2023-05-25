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

    public void LoadData(GameData data) => CoinAmount = data.coinsAmount;
    public void SaveData(ref GameData data) => data.coinsAmount = CoinAmount;
}