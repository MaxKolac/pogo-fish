using UnityEngine;

public class OneJumpPlatform : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>())
            gameObject.SetActive(false);
    }
}
