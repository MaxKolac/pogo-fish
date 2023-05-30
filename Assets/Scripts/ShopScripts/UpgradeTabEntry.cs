using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class UpgradeTabEntry : MonoBehaviour, IDataPersistence
{
    [Header("Script References")]
    [SerializeField] protected ShopCoinCounter shopCoinCounterScript;
    [Header("References")]
    [SerializeField] protected TMP_Text costText;
    [SerializeField] protected UnityEngine.UI.Image levelBarsImage;
    [SerializeField] protected UnityEngine.UI.Image buyButtonImage;
    [SerializeField] protected string upgradeLevelVarName;
    [Header("Sprite Atlases")]
    [SerializeField] protected SpriteAtlas levelBars;
    [SerializeField] protected SpriteAtlas buyButtonStates;

    public int UpgradeLevel { get; protected set; } = 0;
    public int CostPerLevel { get; protected set; } = 20;
    public int UpgradeCost { get; protected set; }
    protected const int MinLevel = 0;
    protected const int MaxLevel = 5;

    void OnEnable()
    {
        Actions.OnUpgradeBought += RefreshSprites;
    }

    void OnDisable()
    {
        Actions.OnUpgradeBought -= RefreshSprites;
    }

    public void AttemptPurchase()
    {
        if (!shopCoinCounterScript.CanAfford(UpgradeCost) || UpgradeLevel == MaxLevel) 
            return;
        UpgradeLevel = Mathf.Clamp(UpgradeLevel + 1, MinLevel, MaxLevel);
        shopCoinCounterScript.SpendCoins(UpgradeCost);
        CalculateUpgradeCost();
        Actions.OnUpgradeBought?.Invoke();
        DataPersistenceManager.Instance.SaveGame();
    }

    public void RefreshSprites()
    {
        levelBarsImage.sprite = levelBars.GetSprite("upgradeitem_lvlbar_" + UpgradeLevel);
        if (UpgradeLevel == MaxLevel)
        {
            buyButtonImage.sprite = buyButtonStates.GetSprite("upgradeitem_button_max");
        }
        else
        {
            buyButtonImage.sprite =
                shopCoinCounterScript.CanAfford(UpgradeCost) ?
                buyButtonStates.GetSprite("upgradeitem_button_buy") :
                buyButtonStates.GetSprite("upgradeitem_button_locked");
        }
    }

    protected void CalculateUpgradeCost()
    {
        UpgradeCost = (UpgradeLevel + 1) * CostPerLevel;
        costText.text = UpgradeLevel < 5 ? $"{UpgradeCost}" : "";
    }

    public void LoadData(GameData data)
    {
        UpgradeLevel = (int)data.GetType().GetField(upgradeLevelVarName).GetValue(data);
        //Debug.Log($"Reflection of {upgradeLevelVarName} returned {UpgradeLevel}");
        CalculateUpgradeCost();
        RefreshSprites();
    }

    public void SaveData(ref GameData data) => data.GetType().GetField(upgradeLevelVarName).SetValue(data, UpgradeLevel);
}
