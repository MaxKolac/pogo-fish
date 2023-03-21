using UnityEngine;

public class VisualPlayer : MonoBehaviour
{
    [SerializeField] private GlobalAttributes globalAttributes;
    [SerializeField] private Rigidbody2D ownRigidbody;
    [SerializeField] private Rigidbody2D ghostRigidbody;
    [SerializeField] private GhostPlayer ghostPlayer;

    [SerializeField] private float accelerationRate = 2f;
    [SerializeField] private float maxHorizontalVelocity = 2f;

    private Vector3 currentTapPosition;

    void FixedUpdate()
    {
        //Teleport player to the other side of screen when he falls out of the screen bounds
        if (transform.position.x < globalAttributes.leftScreenEdge)
            transform.position = new Vector2(globalAttributes.rightScreenEdge, transform.position.y);
        if (transform.position.x > globalAttributes.rightScreenEdge)
            transform.position = new Vector2(globalAttributes.leftScreenEdge, transform.position.y);

        //Update currentTapPosition if user is holding finger
        if (Input.GetMouseButton(0))
        {
            currentTapPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentTapPosition.z = 0;

            //If he does, apply proper velocity, unless Player is already at maxVelocity
            if (Mathf.Abs(ownRigidbody.velocity.x) >= maxHorizontalVelocity) return;
            ownRigidbody.AddForce(
                currentTapPosition.x < globalAttributes.middleOfScreen.x ?
                new Vector2(-1 * accelerationRate, 0) :
                new Vector2(accelerationRate, 0)
            );
        }
        else
        {
            //Else, decelerate - round up velocity lower than 0.25 to 0
            ownRigidbody.velocity =
                Mathf.Abs(ownRigidbody.velocity.x) < 0.25 ?
                new Vector2(0, ownRigidbody.velocity.y) :
                new Vector2(ownRigidbody.velocity.x * 0.99f, ownRigidbody.velocity.y);
        }

        //If platforms are scrolling, stay at heightBarrier
        if (ghostPlayer.isScrolling) transform.position = new Vector2(transform.position.x, globalAttributes.heightBarrier);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ownRigidbody.velocity = new Vector2(ownRigidbody.velocity.x, 0);
        ownRigidbody.AddForce(Vector2.up * globalAttributes.jumpForce, ForceMode2D.Impulse);
        ghostPlayer.Jump();
    }
}
