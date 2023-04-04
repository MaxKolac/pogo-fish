using System;
using UnityEngine;

public static class Actions
{
    /// <summary>
    /// Action trigerred by Platform whenever it should be enqueued by <c>PlatformPooler</c> and disabled.
    /// </summary>
    public static Action<Platform, GameObject> OnPlatformDespawn;
    /// <summary>
    /// Action triggered by HeightSimulator whenever the "scroll down" illusion is being simulated, and therefore, DeltaHeight's value is changing.
    /// </summary>
    public static Action<float> OnDeltaHeightChanged;
    /// <summary>
    /// Action triggered by Player whenever it falls below <c>GlobalAttributes.LowerScreenEdge</c>. Signals that the game has been lost.
    /// </summary>
    public static Action OnGameLost;
}
