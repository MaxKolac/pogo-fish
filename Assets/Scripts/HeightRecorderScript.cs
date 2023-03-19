using UnityEngine;
using UnityEngine.EventSystems;

public class HeightRecorderScript : MonoBehaviour
{
    public Rigidbody2D recorderRigidbody;
    public PlatformPoolerScript platformPooler;
    public Rigidbody2D playerRigidbody;
    public PlayerScript playerScript;

    /// <summary>Difference between last player's Y coordinate from last frame and the current value of it.</summary>
    public float deltaHeight { private set; get; }
    /// <summary>The Y coordinate above which the player causes the game to "scroll up".</summary>
    public float heightBarrier { private set; get; }
    /// <summary>Player's position from last frame.</summary>
    private Vector2 oldPlayerPosition;

    // Start is called before the first frame update
    void Start()
    {
        heightBarrier = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 3f, 0)).y;
        oldPlayerPosition = playerRigidbody.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //heightBarrier
        Debug.DrawLine(new Vector2(-10, heightBarrier), new Vector2(10, heightBarrier), Color.red);

        //Copy player's velocity and position when he's below the barrier
        if (Mathf.Max(playerRigidbody.position.y, recorderRigidbody.position.y) < heightBarrier)
        {
            recorderRigidbody.transform.position = playerRigidbody.position;
            recorderRigidbody.velocity = playerRigidbody.velocity;
            deltaHeight = 0;
            return;
        }

        //If player is above heightBarrier, simulate player's deltaHeight and scroll down all GameObjects.
        //Also keep the player at heightBarrier, unless he's colliding with platform
        deltaHeight = transform.position.y - oldPlayerPosition.y;
        foreach (GameObject obj in platformPooler.GetActivePooledPlatforms())
            obj.transform.position = new Vector2(obj.transform.position.x, Mathf.Max(0, obj.transform.position.y - deltaHeight));
         oldPlayerPosition = transform.position;

        //TODO: :(
        if (playerScript.playerRaycast.collider == null)
            playerRigidbody.position = new Vector2(playerRigidbody.position.x, heightBarrier);
    }
}
