using System;
using UnityEngine;

public enum PlatformType { Default, OneJump, SideWaysMoving }

/// <summary>
/// Base script class of all Platforms.
/// </summary>
public class Platform : MonoBehaviour, IPoolable
{
    public PlatformType Type; 
    public bool DespawnedByPlayer { get; protected set; } = false;

    public Enum GetPoolableType() => Type;

    protected virtual void OnEnable() => DespawnedByPlayer = false;
    protected void Update() => CheckPosition();

    /// <summary>
    /// Checks if the <c>Platform</c> is below screen's lower edge, which means the proper <c>Actions</c> should be invoked.
    /// Include this method in all subclasses' <c>Update()</c> methods, if they are different.
    /// </summary>
    protected void CheckPosition()
    {
        if (transform.position.y < GlobalAttributes.DespawnBarrier)
            Actions.OnPlatformDespawn?.Invoke(this, gameObject);
    }

    /// <param name="collision">The Collision2D object to check.</param>
    /// <param name="checkIfPlayerIsAbove">If true, the analyzed object needs to be above caller's transform to return true.</param>
    /// <param name="checkIfPlayerGoingUp">If true, the analyzed object needs to have positive Y velocity to return true.</param>
    /// <returns>True, if the colliding GameObject has "Player" tag, and if it matches the additional checks</returns>
    protected bool IsCollidingWithPlayer(Collision2D collision, bool checkIfPlayerIsAbove = false, bool checkIfPlayerGoingUp = false)
    {
        return collision != null
            && collision.collider.CompareTag("Player")
            && (!checkIfPlayerIsAbove || collision.collider.transform.position.y > transform.position.y)
            && (!checkIfPlayerGoingUp || collision.collider.GetComponent<Rigidbody2D>().velocity.y <= 0);
    }
}

