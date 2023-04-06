using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private HeightSimulator heightSimulator;
    [SerializeField] private Rigidbody2D ownRigidbody;

    private const float accelerationRate = 4.25f;
    private const float decelerationRate = 0.25f;
    private const float minHorizontalVelocity = 0.35f;
    private const float maxHorizontalVelocity = 10f;
    private const float jumpForce = 10f;

    public bool IsFrozenOnX { private set; get; }
    public bool IsFrozenOnY { private set; get; }

    private Vector3 currentTapPosition;

    void OnEnable()
    {
        ResetToStartingPosition();
    }

    void FixedUpdate()
    {
        if (transform.position.y < GlobalAttributes.LowerScreenEdge && GameManager.CurrentGameState == GameManager.GameState.Playing) 
            Actions.OnGameLost?.Invoke();

        // X Movement System
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
            ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x + accelerationRate, ownRigidbody.velocity.y);
            /*ownRigidbody.AddForce(
                currentTapPosition.x < GlobalAttributes.MiddleOfScreen.x ?
                new Vector2(-1 * accelerationRate, 0) :
                new Vector2(accelerationRate, 0),
                ForceMode2D.Force
            );*/
        }
        else
        {
            //Else, decelerate - round up velocity lower than minHorizontalVelocity to 0
            ownRigidbody.velocity =
                Mathf.Abs(ownRigidbody.velocity.x) < minHorizontalVelocity ?
                new Vector2(0, ownRigidbody.velocity.y) :
                new Vector2(ownRigidbody.velocity.x - (Mathf.Sign(ownRigidbody.velocity.x) * decelerationRate), ownRigidbody.velocity.y);
        }

        // Y Movement system
        //Speed limit
        ownRigidbody.velocity = new Vector2(
            ownRigidbody.velocity.x, 
            Mathf.Clamp(ownRigidbody.velocity.y, -10, jumpForce)
            );

        if (IsFrozenOnY) return;
        if (ownRigidbody.position.y > GlobalAttributes.HeightBarrier && ownRigidbody.velocity.y > 0)
        { 
            heightSimulator.Unfreeze();
            heightSimulator.SetVerticalVelocity(ownRigidbody.velocity.y);
            FreezeOnlyOnY(); 
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (ownRigidbody.velocity.y > 0 || ownRigidbody.position.y <= collision.collider.transform.position.y) return;
        ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, jumpForce);
    }


    public void Freeze()
    {
        IsFrozenOnX = IsFrozenOnY = true;
        ownRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void FreezeOnlyOnY()
    {
        IsFrozenOnY = true;
        ownRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    public void Unfreeze()
    {
        IsFrozenOnX = IsFrozenOnY = false;
        ownRigidbody.WakeUp();
        ownRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void ResetToStartingPosition() => ownRigidbody.position = new Vector2(0, 1.5f);

    public void SetVelocity(Vector2 velocity) => ownRigidbody.velocity = velocity;

    public Vector2 GetVelocity() { return ownRigidbody.velocity; }
}
