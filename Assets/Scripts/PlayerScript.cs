using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public Rigidbody2D rigidBody;
    public BoxCollider2D playerCollider;

    /// <summary>Maximum horizontal velocity the Player can reach.</summary>
    [SerializeField] private float maxVelocity = 10.0f;
    /// <summary>Amount of horizontal force applied to Player per frame when user is holding down their finger.</summary>
    [SerializeField] private float accelerationRate = 2.0f;
    /// <summary>Multiplier of the Vector3.up, whenever Player jumps.</summary>
    [SerializeField] private int jumpForce = 12;

    private float rightScreenEdge;
    private float leftScreenEdge;
    private float middleOfTheScreen;
    private Vector3 currentTapPosition;

    // Start is called before the first frame update
    void Start()
    {
        leftScreenEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        rightScreenEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x;
        middleOfTheScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, 0)).x;
    }

    // Update is called once per frame
    void Update()
    {
        //Teleport player to the other side when he falls out of the screen bounds
        if (transform.position.x < leftScreenEdge)
            transform.position = new Vector3(rightScreenEdge, transform.position.y, transform.position.z);
        
        if (transform.position.x > rightScreenEdge)
            transform.position = new Vector3(leftScreenEdge, transform.position.y, transform.position.z);

        //Update currentTapPosition if user is holding finger
        if (Input.GetMouseButton(0))
        {
            currentTapPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentTapPosition.z = 0;

            //If he does, apply proper velocity, unless Player is already at maxVelocity
            if (Mathf.Abs(rigidBody.velocity.x) >= maxVelocity) return;
            if (currentTapPosition.x < middleOfTheScreen)
                rigidBody.AddForce(new Vector2(-accelerationRate, 0));
            else
                rigidBody.AddForce(new Vector2(accelerationRate, 0));
        }
        //Else, decelerate
        else
        {
            //Round up velocity lower than 0.25 to 0
            if (Mathf.Abs(rigidBody.velocity.x) < 0.25)
            {
                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                return;
            }
            rigidBody.velocity = new Vector2(rigidBody.velocity.x * 0.99f, rigidBody.velocity.y);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (rigidBody.velocity.y > 0 || transform.position.y <= collision.collider.transform.position.y) return;
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }


}
