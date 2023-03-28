using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private HeightSimulator heightSimulator;
    [SerializeField] private Rigidbody2D ownRigidbody;

    private const float accelerationRate = 3.5f;
    private const float maxHorizontalVelocity = 10f;
    private const float jumpForce = 10f;

    public bool UpdatesSuspended { private set; get; } = false;
    private Vector3 currentTapPosition;

    void OnEnable()
    {
        ownRigidbody.position = new Vector2(0, 1.25f);
        UpdatesSuspended = false;
    }

    void FixedUpdate()
    {
        //Teleport player to the other side of screen when he falls out of the screen bounds
        if (transform.position.x < GlobalAttributes.LeftScreenEdge)
            transform.position = new Vector2(GlobalAttributes.RightScreenEdge, transform.position.y);
        if (transform.position.x > GlobalAttributes.RightScreenEdge)
            transform.position = new Vector2(GlobalAttributes.LeftScreenEdge, transform.position.y);

        //Update currentTapPosition if user is holding finger
        if (Input.GetMouseButton(0))
        {
            currentTapPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentTapPosition.z = 0;

            //If he does, apply proper velocity, unless Player is already at maxVelocity
            if (Mathf.Abs(ownRigidbody.velocity.x) >= maxHorizontalVelocity) return;
            ownRigidbody.AddForce(
                currentTapPosition.x < GlobalAttributes.MiddleOfScreen.x ?
                new Vector2(-1 * accelerationRate, 0) :
                new Vector2(accelerationRate, 0)
            );
        }
        else
        {
            //Else, decelerate - round up velocity lower than 0.05 to 0
            ownRigidbody.velocity =
                Mathf.Abs(ownRigidbody.velocity.x) < 0.25 ?
                new Vector2(0, ownRigidbody.velocity.y) :
                new Vector2(ownRigidbody.velocity.x * 0.99f, ownRigidbody.velocity.y);
        }

        if (UpdatesSuspended) return;
        if (ownRigidbody.position.y > GlobalAttributes.HeightBarrier && ownRigidbody.velocity.y > 0)
        { 
            heightSimulator.ResumeUpdates(ownRigidbody.velocity.y);
            SuspendUpdates(); 
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (ownRigidbody.velocity.y > 0 || ownRigidbody.position.y <= collision.collider.transform.position.y) return;
        ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, jumpForce);
    }

    public void SuspendUpdates()
    {
        if (UpdatesSuspended) return;
        UpdatesSuspended = true; 
        ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, 0);
        ownRigidbody.gravityScale = 0;
    }

    public void ResumeUpdates(float verticalVelocity)
    {
        if (!UpdatesSuspended) return;
        UpdatesSuspended = false;
        ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, verticalVelocity);
        ownRigidbody.gravityScale = 1;
    }
}
