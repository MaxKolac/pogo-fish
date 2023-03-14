using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    //public Vector3 mousePosition;
    //public Vector3 worldPosition;
    private Rigidbody2D rigidBody;
    ///<summary> Vectro2.up multiplier for whenever the Player object jumps. </summary>
    [SerializeField] private int jumpForce = 12;
    ///<summary> The current position of the tap. </summary>
    [SerializeField] private Vector3 currentTapPosition;

    public int interpolationFramesCount = 60; // Number of frames to completely interpolate between the 2 positions
    int elapsedFrames = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButton(0)) return;
        currentTapPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentTapPosition.z = 0;
        //transform.position = new Vector3(currentTapPosition.x, transform.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, new Vector3(currentTapPosition.x, transform.position.y, transform.position.z), (float)elapsedFrames / interpolationFramesCount);
        elapsedFrames = (elapsedFrames + 1) % (interpolationFramesCount + 1);  // reset elapsedFrames to zero after it reached (interpolationFramesCount + 1)
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (rigidBody.velocity.y > 0) return;
        rigidBody.velocity = Vector3.zero;
        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }


}
