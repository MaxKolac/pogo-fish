using UnityEngine;

public class Coin : PickableObject
{
    [SerializeField] private Collider2D ownCollider;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (IsColliderPlayer(collision))
        {
            Actions.OnPickableObjectPickedUp?.Invoke(this, gameObject);
            Actions.OnPickableObjectDespawn?.Invoke(Type, gameObject);
        }
    }
}
