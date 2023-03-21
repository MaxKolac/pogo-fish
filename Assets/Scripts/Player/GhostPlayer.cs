using UnityEngine;

public class GhostPlayer : MonoBehaviour
{
    [SerializeField] private GlobalAttributes globalAttributes;
    [SerializeField] private Rigidbody2D ownRigidbody;
    [SerializeField] private Rigidbody2D visualRigidbody;
    [SerializeField] private PlatformPooler platformPooler;

    public bool isScrolling { private set; get; }

    private float deltaHeight;
    private Vector2 oldPosition;

    void Start()
    {
        isScrolling = false;
        deltaHeight = 0;
        oldPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (ownRigidbody.position.y < globalAttributes.heightBarrier)
        {
            isScrolling = false;
            return;
        }
        isScrolling = true;
        deltaHeight = transform.position.y - oldPosition.y;
        platformPooler.ScrollPooledPlatformsDown(Mathf.Max(0f, deltaHeight));
        oldPosition = transform.position;
    }

    public void Jump()
    {
        ownRigidbody.position = new Vector2(ownRigidbody.position.x, 1.25f);
        ownRigidbody.velocity = Vector2.zero;
        ownRigidbody.AddForce(Vector2.up * globalAttributes.jumpForce, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision) => Jump();
}
