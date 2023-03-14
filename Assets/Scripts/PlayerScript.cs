using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    private Rigidbody2D rigidBody;
    [SerializeField] private float maxHorizontalVelocity = 10.0f;
    [SerializeField] private int jumpForce = 12;
    private Vector3 currentTapPosition;
    private int interpolationFramesCount = 60; // Number of frames to completely interpolate between the 2 positions
    private int elapsedFrames = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Teleport player to the other side when he falls out of the screen bounds

        if (!Input.GetMouseButton(0)) return;
        currentTapPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentTapPosition.z = 0;
        //transform.position = new Vector3(currentTapPosition.x, transform.position.y, transform.position.z);

        //transform.position = Vector3.Lerp(transform.position, new Vector3(currentTapPosition.x, transform.position.y, transform.position.z), (float)elapsedFrames / interpolationFramesCount);
        //elapsedFrames = (elapsedFrames + 1) % (interpolationFramesCount + 1);  // reset elapsedFrames to zero after it reached (interpolationFramesCount + 1)

        if (Mathf.Abs(rigidBody.velocity.x) >= maxHorizontalVelocity) return;
        if (currentTapPosition.x < Screen.width / 2)
            rigidBody.AddForce(new Vector2(-1, 0));
        else
            rigidBody.AddForce(new Vector2(1, 0));
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (rigidBody.velocity.y > 0 || transform.position.y <= collision.collider.transform.position.y) return;
        rigidBody.velocity = Vector3.zero;
        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }


}
