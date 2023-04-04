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
        oldPosition.y = GlobalAttributes.HeightBarrier;
        transform.position = new Vector2(ownRigidbody.position.x, GlobalAttributes.HeightBarrier);
        lastVerticalVelocity = 0;
        Freeze();
    }

    void Update()
    {
        if (ownRigidbody.position.y > GlobalAttributes.HeightBarrier)
        {
            //Debug.Log($"DeltaHeight: {deltaHeight}");
            deltaHeight = transform.position.y - oldPosition.y;
            Actions.OnDeltaHeightChanged?.Invoke(Mathf.Max(0, deltaHeight));
            oldPosition = transform.position;
        }
    }

    void FixedUpdate()
    {
        if (IsFrozen) return;
        //If the GhostPlayer is at the apex of his jump, where the sign of vertical velocity flips from positive to negative
        if (lastVerticalVelocity >= 0f && ownRigidbody.velocity.y <= 0f)
        {
            player.Unfreeze();
            player.SetVerticalVelocity(ownRigidbody.velocity.y);
            Freeze();
        }
        lastVerticalVelocity = ownRigidbody.velocity.y;
    }

    public void Freeze()
    {
        if (IsFrozen) return;
        IsFrozen = true;
        ownRigidbody.velocity = Vector2.zero;
        ownRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Unfreeze()
    {
        if (!IsFrozen) return;
        IsFrozen = false;
        ownRigidbody.position = new Vector2(ownRigidbody.position.x, GlobalAttributes.HeightBarrier);
        ownRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    public void SetVerticalVelocity(float verticalVelocity) =>
        ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, verticalVelocity);
    
    public void SetVelocity(Vector2 velocity) => ownRigidbody.velocity = velocity;

    public Vector2 GetVelocity() { return ownRigidbody.velocity; }
}
