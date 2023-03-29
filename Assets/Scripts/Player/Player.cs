using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private HeightSimulator heightSimulator;
    [SerializeField] private Rigidbody2D ownRigidbody;

    private const float accelerationRate = 3.5f;
    private const float decelerationRate = 0.25f;
    private const float minHorizontalVelocity = 0.35f;
    private const float maxHorizontalVelocity = 10f;
    private const float jumpForce = 10f;

    ///<summary>Weaker type of freezing. Sets gravity scale to 0 and sets the velocity to 0.</summary>
    public bool IsFrozen { private set; get; }
    ///<summary>Stronger type of freezing. Completely nulifies any <c>Update()</c> calls and freezes velocity at 0 when <c>true</c>.</summary>
    public bool IsTitleScreenFrozen { private set; get; }
    private Vector3 currentTapPosition;

    void OnEnable()
    {
        ownRigidbody.position = new Vector2(0, 1.25f);
        Unfreeze();
    }

    void FixedUpdate()
    { 
        if (IsTitleScreenFrozen)
        {
            ownRigidbody.velocity = Vector2.zero;
            return;
        }

        //Speed limit
        ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, Mathf.Clamp(ownRigidbody.velocity.y, -90, jumpForce));

        //Teleport player to the other side of screen when he falls out of the screen bounds
        if (transform.position.x < GlobalAttributes.LeftScreenEdge)
            transform.position = new Vector2(GlobalAttributes.RightScreenEdge, transform.position.y);
        if (transform.position.x > GlobalAttributes.RightScreenEdge)
            transform.position = new Vector2(GlobalAttributes.LeftScreenEdge, transform.position.y);

        if (Input.GetMouseButton(0))
        {
            //Update currentTapPosition and accelerate if user is holding finger
            currentTapPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentTapPosition.z = 0;

            if (Mathf.Abs(ownRigidbody.velocity.x) >= maxHorizontalVelocity) return;
            ownRigidbody.AddForce(
                currentTapPosition.x < GlobalAttributes.MiddleOfScreen.x ?
                new Vector2(-1 * accelerationRate, ownRigidbody.velocity.y) :
                new Vector2(accelerationRate, ownRigidbody.velocity.y)
            );
        }
        else
        {
            //Else, decelerate - round up velocity lower than minHorizontalVelocity to 0
            ownRigidbody.velocity =
                Mathf.Abs(ownRigidbody.velocity.x) < minHorizontalVelocity ?
                new Vector2(0, ownRigidbody.velocity.y) :
                new Vector2(ownRigidbody.velocity.x - (Mathf.Sign(ownRigidbody.velocity.x) * decelerationRate), ownRigidbody.velocity.y);
        }

        if (IsFrozen) return;
        if (ownRigidbody.position.y > GlobalAttributes.HeightBarrier && ownRigidbody.velocity.y > 0)
        { 
            heightSimulator.Unfreeze();
            heightSimulator.TransferVelocity(ownRigidbody.velocity.y);
            Freeze(); 
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (ownRigidbody.velocity.y > 0 || ownRigidbody.position.y <= collision.collider.transform.position.y) return;
        ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, jumpForce);
    }

    public void Freeze()
    {
        if (IsFrozen) return;
        IsFrozen = true; 
        ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, 0);
        ownRigidbody.gravityScale = 0;
    }

    public void Unfreeze()
    {
        if (!IsFrozen) return;
        IsFrozen = false;
        ownRigidbody.gravityScale = 1;
    }

    public void TransferVelocity(float verticalVelocity)
    {
        ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, verticalVelocity);
    }

    public void TitleScreenFreeze()
    {
        if (IsTitleScreenFrozen) return;
        Freeze();
        IsTitleScreenFrozen = true;
    }

    public void TitleScreenUnfreeze()
    {
        if (!IsTitleScreenFrozen) return;
        Unfreeze();
        IsTitleScreenFrozen = false;
    }
}
