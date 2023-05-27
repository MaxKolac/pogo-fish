using UnityEngine;

public class SpringBoost : PickableObject
{
    [SerializeField] private Collider2D ownCollider;

    void OnTriggerStay2D(Collider2D collider)
    {
        if (IsColliderPlayer(collider))
        {
            Actions.OnPickableObjectPickedUp?.Invoke(this, gameObject);
            Actions.OnPickableObjectDespawn?.Invoke(Type, gameObject);
        }
    }
}
