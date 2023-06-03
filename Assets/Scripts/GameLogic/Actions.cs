using System;
using UnityEngine;

public static class Actions
{
    //Gameplay Actions
    /// <summary>
    /// Action triggered by HeightSimulator whenever the "scroll down" illusion is being simulated, and therefore, DeltaHeight's value is changing.
    /// </summary>
    public static Action<float> OnDeltaHeightChanged;
    /// <summary>
    /// Action trigerred by Platform whenever it should be enqueued by <c>PlatformPooler</c> and disabled.
    /// </summary>
    public static Action<PlatformType, GameObject> OnPlatformDespawn;
    /// <summary>
    /// Action trigerred by a PickableObject whenever it should enqueued by PickableObjectPooler and disabled.
    /// </summary>
    public static Action<PickableObjectType, GameObject> OnPickableObjectDespawn;
    /// <summary>
    /// Action trigerred when player collides with a PickableObject and should acquire its effects.
    /// </summary>
    public static Action<PickableObject, GameObject> OnPickableObjectPickedUp;
    /// <summary>
    /// Action triggered when a picked up upgrade had a duration, and that duration just expired.
    /// </summary>
    public static Action OnTimedUpgradeExpires;

    //Game state change Actions
    /// <summary>
    /// Action trigger when user click Pause button during gameplay.
    /// </summary>
    public static Action OnGamePaused;
    /// <summary>
    /// Action trigger when user returns back to playing after pausing the game.
    /// </summary>
    public static Action OnGameUnpaused;
    /// <summary>
    /// Action triggered when user clicks "Quit To Menu" button in Pause screen. Similar to OnGameLost, but not the same.
    /// </summary>
    public static Action OnGameAbandoned;
    /// <summary>
    /// Action triggered by Player whenever it falls below <c>GlobalAttributes.LowerScreenEdge</c>. Signals that the game has been lost.
    /// </summary>
    public static Action OnGameLost;

    //Shop Actions
    /// <summary>
    /// Action triggered whenever the user successfully clicks any skin in the shop. Other Skins should refresh their sprites to show appropriate sprite.
    /// </summary>
    public static Action OnSkinClicked;
    /// <summary>
    /// Action trigerred whenever the user equips an unlocked Skin. Other equipped Skin should set their Status to Unlocked.
    /// </summary>
    public static Action<string> OnSkinEquipped;
    /// <summary>
    /// Action triggered whenever the user succesfully purchases an upgrade. Other UpgradeItem objects should refresh their sprites to check affordability.
    /// </summary>
    public static Action OnUpgradeClicked;
}
