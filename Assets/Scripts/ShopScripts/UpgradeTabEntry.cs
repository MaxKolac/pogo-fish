using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class UpgradeTabEntry : MonoBehaviour, IDataPersistence
{
    [Header("Script References")]
    [SerializeField] private ShopCoinCounter shopCoinCounterScript;
    [Header("Description Settings")]
    [SerializeField] private string effectUnit;
    [SerializeField] private float effectImprovementPerLevel;
    [Header("References")]
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text descText;
    [SerializeField] private UnityEngine.UI.Image levelBarsImage;
    [SerializeField] private UnityEngine.UI.Image buyButtonImage;
    [SerializeField] private string upgradeLevelVarName;
    [Header("Sprite Atlases")]
    [SerializeField] private SpriteAtlas levelBars;
    [SerializeField] private SpriteAtlas buyButtonStates;

    public int UpgradeLevel { get; private set; } = 0;
    public int CostPerLevel { get; private set; } = 20;
    public int UpgradeCost { get; private set; }
    private const int MinLevel = 0;
    private const int MaxLevel = 5;

    void OnEnable() => Actions.OnUpgradeClicked += RefreshSprites;
    void OnDisable() => Actions.OnUpgradeClicked -= RefreshSprites;

    public void AttemptPurchase()
    {
        if (!shopCoinCounterScript.CanAfford(UpgradeCost) || UpgradeLevel == MaxLevel) 
            return;
        UpgradeLevel = Mathf.Clamp(UpgradeLevel + 1, MinLevel, MaxLevel);
        shopCoinCounterScript.SpendCoins(UpgradeCost);
        CalculateUpgradeCost();
        Actions.OnUpgradeClicked?.Invoke();
        DataPersistenceManager.Instance.SaveGame();
    }

    public void RefreshSprites()
    {
        levelBarsImage.sprite = levelBars.GetSprite("upgradeitem_lvlbar_" + UpgradeLevel);
        descText.text = $"+{UpgradeLevel * effectImprovementPerLevel}{effectUnit}";
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

    private void CalculateUpgradeCost()
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
