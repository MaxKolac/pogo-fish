using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GlobalAttributes globalAttributes;
    [SerializeField] private Rigidbody2D ownRigidbody;
    [SerializeField] private HeightSimulator heightSimulator;

    private float accelerationRate = 3.5f;
    private float maxHorizontalVelocity = 10f;

    public bool UpdatesSuspended { private set; get; } = false;
    private Vector3 currentTapPosition;

    void FixedUpdate()
    {
        //Teleport player to the other side of screen when he falls out of the screen bounds
        if (transform.position.x < globalAttributes.LeftScreenEdge)
            transform.position = new Vector2(globalAttributes.RightScreenEdge, transform.position.y);
        if (transform.position.x > globalAttributes.RightScreenEdge)
            transform.position = new Vector2(globalAttributes.LeftScreenEdge, transform.position.y);

        //Update currentTapPosition if user is holding finger
        if (Input.GetMouseButton(0))
        {
            currentTapPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentTapPosition.z = 0;

            //If he does, apply proper velocity, unless Player is already at maxVelocity
            if (Mathf.Abs(ownRigidbody.velocity.x) >= maxHorizontalVelocity) return;
            ownRigidbody.AddForce(
                currentTapPosition.x < globalAttributes.MiddleOfScreen.x ?
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
        if (ownRigidbody.position.y > globalAttributes.HeightBarrier && ownRigidbody.velocity.y > 0)
        { 
            heightSimulator.ResumeUpdates(ownRigidbody.velocity.y);
            SuspendUpdates(); 
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (ownRigidbody.velocity.y > 0 || ownRigidbody.position.y <= collision.collider.transform.position.y) return;
        ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, globalAttributes.JumpForce);
        //ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, 0);
        //ownRigidbody.AddForce(Vector2.up * globalAttributes.jumpForce, ForceMode2D.Impulse);
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
