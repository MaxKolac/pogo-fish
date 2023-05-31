using System;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class SkinTabEntry : MonoBehaviour, IDataPersistence
{
    public int Cost;
    [Header("Script References")]
    [SerializeField] private ShopCoinCounter shopCoinCounterScript;
    [Header("References")]
    [SerializeField] private TMP_Text costText;
    [SerializeField] private UnityEngine.UI.Image shopTabEntryImage;
    [SerializeField] private string skinVarName;
    [SerializeField] private SpriteAtlas skinTabEntryStates;

    public SkinStatus Status { get; private set; } = SkinStatus.Locked;

    void OnEnable()
    {
        Actions.OnSkinClicked += RefreshSprites;
        Actions.OnSkinEquipped += Dequip;
    }

    void OnDisable()
    {
        Actions.OnSkinClicked -= RefreshSprites;
        Actions.OnSkinEquipped -= Dequip;
    }

    public void PurchaseButtonPressed()
    {
        switch (Status)
        {
            case SkinStatus.Affordable:
                Status = SkinStatus.Unlocked;
                shopCoinCounterScript.SpendCoins(Cost);
                DataPersistenceManager.Instance.SaveGame();
                break;
            case SkinStatus.Unlocked:
                Status = SkinStatus.Equipped;
                Actions.OnSkinEquipped?.Invoke(skinVarName);
                DataPersistenceManager.Instance.SaveGame();
                break;
            default:
                break;
        }
        Actions.OnSkinClicked?.Invoke();
    }

    public void Dequip(string equippedSkinVarName)
    {
        if (Status != SkinStatus.Equipped || equippedSkinVarName == skinVarName) 
            return;
        Status = SkinStatus.Unlocked;
    }

    public void RefreshSprites()
    {
        if (Status == SkinStatus.Locked || Status == SkinStatus.Affordable)
        {
            if (shopCoinCounterScript.CanAfford(Cost))
                Status = SkinStatus.Affordable;
            else
                Status = SkinStatus.Locked;
        }
        switch (Status)
        {
            case SkinStatus.Locked:
                shopTabEntryImage.sprite = skinTabEntryStates.GetSprite("skin_locked");
                costText.text = $"{Cost}";
                break;
            case SkinStatus.Affordable:
                shopTabEntryImage.sprite = skinTabEntryStates.GetSprite("skin_affordable");
                costText.text = $"{Cost}";
                break;
            case SkinStatus.Unlocked:
                shopTabEntryImage.sprite = skinTabEntryStates.GetSprite("skin_unlocked");
                costText.text = "";
                break;
            case SkinStatus.Equipped:
                shopTabEntryImage.sprite = skinTabEntryStates.GetSprite("skin_equipped");
                costText.text = "";
                break;
        }
    }

    public void LoadData(GameData data)
    {
        Status = (SkinStatus)data.GetType().GetField(skinVarName).GetValue(data);
        //Debug.Log($"Reflection of {skinVarName} returned {Status}");
        RefreshSprites();
    }

    public void SaveData(ref GameData data) => data.GetType().GetField(skinVarName).SetValue(data, (int)Status);
}

[Serializable]
public enum SkinStatus
{
    /// <summary>
    /// Skin is locked and user can't afford purchasing it.
    /// </summary>
    Locked, 
    /// <summary>
    /// Skin is locked but if the user chooses to, he can afford a purchase of it.
    /// </summary>
    Affordable, 
    /// <summary>
    /// Skin was purchased and is available to be equipped.
    /// </summary>
    Unlocked, 
    /// <summary>
    /// Skin is currently purchased and equipped.
    /// </summary>
    Equipped
}
