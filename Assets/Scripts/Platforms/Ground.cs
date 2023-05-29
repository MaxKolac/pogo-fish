using UnityEngine;

public class Ground : MonoBehaviour
{
    void OnEnable()
    {
        Actions.OnDeltaHeightChanged += ScrollDown;
    }

    void ScrollDown(float deltaHeight)
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - deltaHeight);
    }

    void Update()
    {
        if (transform.position.y < GlobalAttributes.DespawnBarrier)
            gameObject.SetActive(false);
    }

    void OnDisable()
    {
        Actions.OnDeltaHeightChanged -= ScrollDown;
    }
}
