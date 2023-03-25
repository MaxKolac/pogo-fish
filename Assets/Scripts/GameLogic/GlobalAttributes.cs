using UnityEngine;

public class GlobalAttributes : MonoBehaviour
{
    //GhostPlayer
    public float HeightBarrier { private set; get; }
    public float JumpForce { private set; get; }

    //Screen
    public float LeftScreenEdge { private set; get; }
    public float LowerScreenEdge { private set; get; }
    public float RightScreenEdge { private set; get; }
    public float UpperScreenEdge { private set; get; }
    public Vector2 MiddleOfScreen { private set; get; }
    
    void OnEnable()
    {
        HeightBarrier = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2f, 0)).y;
        JumpForce = 10f;

        LeftScreenEdge = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x;
        LowerScreenEdge = Camera.main.ScreenToWorldPoint(Vector2.zero).y;
        RightScreenEdge = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
        UpperScreenEdge = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
        MiddleOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2f));
    }

    void Update()
    {
        Debug.DrawLine(new Vector2(-10, HeightBarrier), new Vector2(10, HeightBarrier), Color.red);
    }
}
