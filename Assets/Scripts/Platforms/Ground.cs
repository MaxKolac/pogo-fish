using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private GlobalAttributes globalAttributes;
    [SerializeField] private SpriteRenderer ownRenderer;

    // Start is called before the first frame update
    void OnEnable()
    {
        transform.position = new Vector2(0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < globalAttributes.LowerScreenEdge - ownRenderer.bounds.size.y)
            enabled = false;
    }
}
