using UnityEngine;

public class PickableObject : MonoBehaviour
{
    [SerializeField] protected Collider2D ownCollider;
    public PickableObjectType Type;
    public bool IsAttractedByMagnet;

    protected void Update()
    {
        if (!IsAttractedByMagnet)
            CheckPosition();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsColliderPlayer(collision))
        {
            Actions.OnPickableObjectPickedUp?.Invoke(this, gameObject);
            //Actions.OnPickableObjectDespawn?.Invoke(Type, gameObject);
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
        return (collider != null && collider.CompareTag("Player"));
    }
}

public enum PickableObjectType
{
    Coin, SpringBoost, Magnet
}