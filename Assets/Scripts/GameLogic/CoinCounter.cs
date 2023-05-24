using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour, IDataPersistence
{
    [SerializeField] private TMP_Text coinText;
    public int CurrentCoinAmount { private set; get; } = 0;

    private void OnEnable()
    {
        Actions.OnPickableObjectPickedUp += IncreaseAmount;
        CurrentCoinAmount = 0;
    }

    void OnDisable()
    {
        Actions.OnPickableObjectPickedUp -= IncreaseAmount;
    }

    private void IncreaseAmount(PickableObject pickableObj, GameObject gameObj)
    {
        if (pickableObj.Type == PickableObjectType.Coin)
            CurrentCoinAmount++;
    }

    private void Update()
    {
        coinText.text = $"{CurrentCoinAmount}";
    }

    public void LoadData(GameData data) => CurrentCoinAmount = data.coinsAmount;
    public void SaveData(ref GameData data) => data.coinsAmount = CurrentCoinAmount;
}
