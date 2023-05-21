using UnityEngine;

public class SideWaysMovingPlatform : Platform
{
    private Vector2 initialPosition;
    private const float travelDistance = 1.25f;
    private const float travelSpeed = 0.025f;
    private int travelDirection;

    void OnEnable()
    {
        initialPosition = transform.position;
        travelDirection = transform.position.x < GlobalAttributes.MiddleOfScreen.x ? 1 : -1;
    }

    void FixedUpdate()
    {
        if (GameManager.CurrentGameState != GameState.Playing) return;
        if (travelDirection == 1 && transform.position.x > initialPosition.x + travelDistance)
        {
            travelDirection = -1;
            return;
        }
        if (travelDirection == -1 && transform.position.x < initialPosition.x - travelDistance)
        {
            travelDirection = 1;
            return;
        }
        transform.position = new Vector2(transform.position.x + travelDirection * travelSpeed, transform.position.y);
    }
}
