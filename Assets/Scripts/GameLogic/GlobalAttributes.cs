using UnityEngine;

public class GlobalAttributes
{
    /// <summary>
    /// If Player reaches this Y coordinate when jumping, <c>HeightSimulator</c> will begin simulating the game's "scrolling down" illusion.
    /// </summary>
    public static float HeightBarrier { private set; get; } = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 3f, 0)).y;

    /// <summary>
    /// Total height "simulated" by the Height Simulator during one game. 
    /// </summary>
    public static float TotalGainedHeight = 0;

    //Screen
    public static float LeftScreenEdge { private set; get; } = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x;
    public static float LowerScreenEdge { private set; get; } = Camera.main.ScreenToWorldPoint(Vector2.zero).y;
    public static float RightScreenEdge { private set; get; } = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
    public static float UpperScreenEdge { private set; get; } = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
    public static Vector2 MiddleOfScreen { private set; get; } = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2f));
    public static float ScreenWorldWidth { private set; get; } = Mathf.Abs(LeftScreenEdge) + Mathf.Abs(RightScreenEdge);
    public static float ScreenWorldHeight { private set; get; } = Mathf.Abs(LowerScreenEdge) + Mathf.Abs(UpperScreenEdge);
}
