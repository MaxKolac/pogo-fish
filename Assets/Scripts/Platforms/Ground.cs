using UnityEngine;

public class Ground : MonoBehaviour
{
    void OnEnable()
    {
        transform.position = new Vector2(0f, 0.5f);
    }

    void Update()
    {
        if (transform.position.y < GlobalAttributes.LowerScreenEdge)
            gameObject.SetActive(false);
    }
}
