using UnityEngine;

public class Ground : MonoBehaviour
{
    void OnEnable()
    {
        Actions.OnDeltaHeightChanged += ScrollDown;
        transform.position = new Vector2(0f, 0.5f);
    }

    void ScrollDown(float deltaHeight)
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - Mathf.Max(0, deltaHeight));
    }

    void OnDisable()
    {
        Actions.OnDeltaHeightChanged -= ScrollDown;
    }

    void Update()
    {
        if (transform.position.y < GlobalAttributes.LowerScreenEdge)
            gameObject.SetActive(false);
    }
}
