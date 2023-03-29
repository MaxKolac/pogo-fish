using UnityEngine;

public class HeightSimulator : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Rigidbody2D ownRigidbody;

    public bool UpdatesSuspended { private set; get; }

    private Vector2 oldPosition;
    private float deltaHeight;
    private float lastVerticalVelocity;

    void OnEnable()
    {
        UpdatesSuspended = false;
        deltaHeight = 0;
        //GlobalAttributes.DeltaHeight = 0;
        //GlobalAttributes.LastTotalDeltaHeight = 0;
        oldPosition = transform.position;
        lastVerticalVelocity = 0;
        SuspendUpdates();
    }

    void FixedUpdate()
    {
        if (ownRigidbody.position.y > GlobalAttributes.HeightBarrier)
        {
            //GlobalAttributes.DeltaHeight = transform.position.y - oldPosition.y;
            deltaHeight = transform.position.y - oldPosition.y;
            //GlobalAttributes.LastTotalDeltaHeight = Mathf.Max(GlobalAttributes.LastTotalDeltaHeight, transform.position.y - GlobalAttributes.HeightBarrier);
            //Actions.OnDeltaHeightChanged?.Invoke(GlobalAttributes.DeltaHeight);
            Actions.OnDeltaHeightChanged?.Invoke(deltaHeight);
            oldPosition = transform.position;
        }
            
        if (UpdatesSuspended) return;
        //If the GhostPlayer is at the apex of his jump, where the sign of vertical velocity flips from positive to negative
        if (lastVerticalVelocity >= 0f && ownRigidbody.velocity.y <= 0f)
        {
            player.ResumeUpdates(ownRigidbody.velocity.y);
            SuspendUpdates();
        }
        lastVerticalVelocity = ownRigidbody.velocity.y;
    }

    public void SuspendUpdates()
    {
        if (UpdatesSuspended) return;
        UpdatesSuspended = true;
        ownRigidbody.velocity = Vector2.zero;
        ownRigidbody.gravityScale = 0;
        //GlobalAttributes.LastTotalDeltaHeight = 0;
    }

    public void ResumeUpdates(float verticalVelocity)
    {
        if (!UpdatesSuspended) return;
        UpdatesSuspended = false;
        //ownRigidbody.position = new Vector2(ownRigidbody.position.x, GlobalAttributes.HeightBarrier);
        ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, verticalVelocity);
        ownRigidbody.gravityScale = 1;
    }
}
