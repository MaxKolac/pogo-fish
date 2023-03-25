using UnityEngine;

public class HeightSimulator : MonoBehaviour
{
    [SerializeField] private GlobalAttributes globalAttributes;
    [SerializeField] private Rigidbody2D ownRigidbody;
    [SerializeField] private Player player;

    [SerializeField] private PlatformPooler platformPooler;
    [SerializeField] private ScoreCounter scoreCounter;

    public bool UpdatesSuspended { private set; get; }

    private float deltaHeight;
    private Vector2 oldPosition;
    private float lastVerticalVelocity;

    void OnEnable()
    {
        UpdatesSuspended = false;
        deltaHeight = 0;
        oldPosition = transform.position;
        lastVerticalVelocity = 0;
        SuspendUpdates();
    }

    void FixedUpdate()
    {
        if (ownRigidbody.position.y > globalAttributes.HeightBarrier)
        {
            deltaHeight = transform.position.y - oldPosition.y;
            platformPooler.ScrollPlatformsDown(Mathf.Max(0f, deltaHeight));
            scoreCounter.IncreaseScore(Mathf.Max(0f, deltaHeight));
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
    }

    public void ResumeUpdates(float verticalVelocity)
    {
        if (!UpdatesSuspended) return;
        UpdatesSuspended = false;
        ownRigidbody.position = new Vector2(ownRigidbody.position.x, globalAttributes.HeightBarrier);
        ownRigidbody.velocity = new Vector2(0, verticalVelocity);
        ownRigidbody.gravityScale = 1;
    }
}
