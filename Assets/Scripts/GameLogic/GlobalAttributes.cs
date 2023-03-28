using UnityEngine;

public class GlobalAttributes : MonoBehaviour
{
    //heightSimulator
    /// <summary>
    /// If Player reaches this Y coordinate when jumping, <c>HeightSimulator</c> will begin simulating the game's "scrolling down" illusion.
    /// </summary>
    public static float HeightBarrier { private set; get; }
    /// <summary>
    /// The difference between last frame's and current frame's <c>HeightSimulator</c> position. Only non-zero if Player reaches the <c>HeightBarrier</c>.
    /// </summary>
    //public static float DeltaHeight;
    /// <summary>
    /// The total simulated height of Player's last jump, counted from <c>HeightBarrier</c>.
    /// </summary>
    //public static float LastTotalDeltaHeight;

    //Screen
    public static float LeftScreenEdge { private set; get; }
    public static float LowerScreenEdge { private set; get; }
    public static float RightScreenEdge { private set; get; }
    public static float UpperScreenEdge { private set; get; }
    public static Vector2 MiddleOfScreen { private set; get; }
    public static float ScreenWorldWidth { private set; get; }
    public static float ScreenWorldHeight { private set; get; }
    
    void OnEnable()
    {
        HeightBarrier = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2f, 0)).y;
        //DeltaHeight = 0f;
        //LastTotalDeltaHeight = 0f;

        LeftScreenEdge = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x;
        LowerScreenEdge = Camera.main.ScreenToWorldPoint(Vector2.zero).y;
        RightScreenEdge = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
        UpperScreenEdge = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
        MiddleOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2f));
        ScreenWorldWidth = Mathf.Abs(LeftScreenEdge) + Mathf.Abs(RightScreenEdge);
        ScreenWorldHeight = Mathf.Abs(LowerScreenEdge) + Mathf.Abs(UpperScreenEdge);
    }

    void Update()
    {
        Debug.DrawLine(new Vector2(-10, HeightBarrier), new Vector2(10, HeightBarrier), Color.red);
    }
}
