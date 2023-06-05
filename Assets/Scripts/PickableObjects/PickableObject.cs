using System;
using UnityEngine;

public class PickableObject : MonoBehaviour, IPoolable
{
    [SerializeField] protected Collider2D ownCollider;
    public PickableObjectType Type;
    public bool IsAttractedByMagnet;

    public Enum GetPoolableType() => Type;

    protected void Update()
    {
        if (!IsAttractedByMagnet)
            CheckPosition();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsColliderPicker(collision))
        {
            Actions.OnPickableObjectPickedUp?.Invoke(this, gameObject);
            Actions.OnPickableObjectDespawn?.Invoke(Type, gameObject);
        }
    }

    /// <summary>
    /// Checks if the <c>PickableObject</c> is below screen's lower edge, which means the proper <c>Actions</c> should be invoked.
    /// Include this method in all subclasses' <c>Update()</c> methods, if they are different.
    /// </summary>
    protected void CheckPosition()
    {
        if (transform.position.y < GlobalAttributes.DespawnBarrier)
            Actions.OnPickableObjectDespawn?.Invoke(Type, gameObject);
    }

    /// <summary>
    /// Returns "true" if the colliding object has the "Player" tag.
    /// </summary>
    protected bool IsColliderPlayer(Collider2D collider)
    {
        //Debug.Log("Collided with Player!");
        return (collider != null && collider.CompareTag("Player"));
    }

    /// <summary>
    /// Returns "true" if the colliding object has the "Picker" tag.
    /// </summary>
    protected bool IsColliderPicker(Collider2D collider)
    {
        //Debug.Log("Collided with Picker!");
        return (collider != null && collider.CompareTag("Picker"));
    }
}

public enum PickableObjectType
{
    Coin, SpringBoost, Magnet, ScoreMultiplier
}