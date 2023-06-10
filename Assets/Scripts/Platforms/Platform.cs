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
    public bool PlayerWasAboveMe { get; protected set; } = false;
    protected Player playerScript;
    [SerializeField] protected Collider2D ownCollider;

    protected void Awake() => playerScript = GameObject.Find("Player").GetComponent<Player>();

    protected virtual void OnEnable()
    {
        DespawnedByPlayer = false;
        PlayerWasAboveMe = false;
    }

    protected virtual void Update() => CheckPosition();

    /// <summary>
    /// Checks if the <c>Platform</c> is below screen's lower edge, which means the proper <c>Actions</c> should be invoked.
    /// It also checks whether or not the player was ever above the Platform.
    /// Include this method in all subclasses' <c>Update()</c> methods, if they are different.
    /// </summary>
    protected void CheckPosition()
    {
        PlayerWasAboveMe = PlayerWasAboveMe || (playerScript.GetLowerBoundCoordinate() >= GetUpperBoundCoordinate());
        if (transform.position.y < GlobalAttributes.DespawnBarrier)
            Actions.OnPlatformDespawn?.Invoke(this, gameObject);
    }

    protected float GetUpperBoundCoordinate() => ownCollider.bounds.center.y + ownCollider.bounds.extents.y;

    protected bool IsCollisionWithPlayerValid(Collision2D collision)
    {
        Rigidbody2D colliderRigidbody = collision.collider.GetComponent<Rigidbody2D>();
        Collider2D collider = collision.collider;
        Bounds colliderBounds = collider.bounds;
        Bounds ownBounds = ownCollider.bounds;
        
        //if only platform effector were better, dear unity lord
        bool result = collider != null
            && collider.CompareTag("Player")
            && (colliderBounds.center.y /*- colliderBounds.extents.y*/ >= ownBounds.center.y /*+ ownBounds.extents.y*/)
            && PlayerWasAboveMe
            //&& colliderBounds.center.y - colliderBounds.extents.y <= GetUpperBoundCoordinate()
            && (colliderRigidbody.velocity.y <= 0);
        /*Debug.Log($"Collision with player. wasHeAbove?: {PlayerWasAboveMe}, " +
            //$"colliderBoundCalc: {colliderBounds.center.y - colliderBounds.extents.y} <= upperBoundCord: {GetUpperBoundCoordinate()}, " +
            $"colliderBoundCenter: {colliderBounds.center.y} >= ownBoundCenter: {ownBounds.center.y}, " +
            $"colliderVelocityCheck: {colliderRigidbody.velocity.y} <= 0, " +
            $"methodResult: {result}");*/
        /*Debug.Log($"Collision with platform detected!" +
            $"colliderBoundCalc: {colliderBounds.center.y *//*- colliderBounds.extents.y*//*} >= ownBoundCalc: {ownBounds.center.y *//*+ ownBounds.extents.y*//*}, " +
            $"colliderVelocity: {colliderRigidbody.velocity.y} <= 0, " +
            $"methodResult: {result}");*/
        return result;
    }

    public Enum GetPoolableType() => Type;
}

