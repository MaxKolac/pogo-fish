using UnityEngine;

public class GlobalAttributes : MonoBehaviour
{
    //GhostPlayer
    public float heightBarrier { private set; get; }
    public float jumpForce { private set; get; }

    //Screen
    public float leftScreenEdge { private set; get; }
    public float lowerScreenEdge { private set; get; }
    public float rightScreenEdge { private set; get; }
    public float upperScreenEdge { private set; get; }
    public Vector2 middleOfScreen { private set; get; }
    
    //PlatformPooler
    //public float platformTeleportBarrier { private set; get; }

    void Start()
    {
        heightBarrier = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 3f, 0)).y;
        jumpForce = 10f;

        leftScreenEdge = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x;
        lowerScreenEdge = Camera.main.ScreenToWorldPoint(Vector2.zero).y;
        rightScreenEdge = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
        upperScreenEdge = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
        middleOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2f));

        // = lowerScreenEdge - 5f;
    }

    void Update()
    {
        Debug.DrawLine(new Vector2(-10, heightBarrier), new Vector2(10, heightBarrier), Color.red);
        //Debug.DrawLine(new Vector2(-10, platformTeleportBarrier), new Vector2(10, platformTeleportBarrier), Color.red);
    }
}
