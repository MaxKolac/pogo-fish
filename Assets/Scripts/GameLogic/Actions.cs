using System;
using UnityEngine;

public static class Actions
{
        //Gameplay Actions
    /// <summary>
    /// Action trigerred by Platform whenever it should be enqueued by <c>PlatformPooler</c> and disabled.
    /// </summary>
    public static Action<PlatformType, GameObject> OnPlatformDespawn;
    /// <summary>
    /// Action trigerred by a PickableObject whenever it should enqueued by PickableObjectPooler and disabled.
    /// </summary>
    public static Action<PickableObjectType, GameObject> OnPickableObjectDespawn;
    /// <summary>
    /// Action triggered by HeightSimulator whenever the "scroll down" illusion is being simulated, and therefore, DeltaHeight's value is changing.
    /// </summary>
    public static Action<float> OnDeltaHeightChanged;
    /// <summary>
    /// Action trigerred when player collides with a PickableObject and should acquire its effects.
    /// </summary>
    public static Action<PickableObject, GameObject> OnPickableObjectPickedUp;
    /// <summary>
    /// Action triggered by Player whenever it falls below <c>GlobalAttributes.LowerScreenEdge</c>. Signals that the game has been lost.
    /// </summary>
    public static Action OnGameLost;

        //Shop Actions
    /// <summary>
    /// Action triggered whenever the user succesfully purchases an upgrade. Other UpgradeItem objects should refresh their sprites to check affordability.
    /// </summary>
    public static Action OnUpgradeBought;
}
