using UnityEngine;

public class HeightSimulator : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Rigidbody2D ownRigidbody;

    public bool IsFrozen { private set; get; }

    private Vector2 oldPosition;
    private float deltaHeight;
    private float lastVerticalVelocity;

    void OnEnable()
    {
        deltaHeight = 0;
        oldPosition = transform.position;
        lastVerticalVelocity = 0;
        Freeze();
    }

    void FixedUpdate()
    {
        if (ownRigidbody.position.y > GlobalAttributes.HeightBarrier)
        {
            deltaHeight = transform.position.y - oldPosition.y;
            Actions.OnDeltaHeightChanged?.Invoke(deltaHeight);
            oldPosition = transform.position;
        }

        if (IsFrozen) return;
        //If the GhostPlayer is at the apex of his jump, where the sign of vertical velocity flips from positive to negative
        if (lastVerticalVelocity >= 0f && ownRigidbody.velocity.y <= 0f)
        {
            player.Unfreeze();
            player.TransferVelocity(ownRigidbody.velocity.y);
            Freeze();
        }
        lastVerticalVelocity = ownRigidbody.velocity.y;
    }

    public void Freeze()
    {
        if (IsFrozen) return;
        IsFrozen = true;
        ownRigidbody.velocity = Vector2.zero;
        ownRigidbody.gravityScale = 0;
    }

    public void Unfreeze()
    {
        if (!IsFrozen) return;
        IsFrozen = false;
        ownRigidbody.position = new Vector2(ownRigidbody.position.x, GlobalAttributes.HeightBarrier);
        ownRigidbody.gravityScale = 1;
    }

    public void TransferVelocity(float verticalVelocity)
    {
        ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, verticalVelocity);
    }
}
