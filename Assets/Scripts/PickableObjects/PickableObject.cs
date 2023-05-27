using UnityEngine;

public abstract class PickableObject : MonoBehaviour
{
    public PickableObjectType Type;

    protected void Update()
    {
        CheckPosition();
    }

    /// <summary>
    /// Checks if the <c>PickableObject</c> is below screen's lower edge, which means the proper <c>Actions</c> should be invoked.
    /// Include this method in all subclasses' <c>Update()</c> methods, if they are different.
    /// </summary>
    protected void CheckPosition()
    {
        if (transform.position.y < GlobalAttributes.LowerScreenEdge)
            Actions.OnPickableObjectDespawn?.Invoke(Type, gameObject);
    }

    /// <summary>
    /// Returns "true" if the colliding object has the "Player" tag.
    /// </summary>
    protected bool IsColliderPlayer(Collider2D collider)
    {
        return (collider != null && collider.CompareTag("Player"));
    }
}

public enum PickableObjectType
{
    Coin, SpringBoost
}