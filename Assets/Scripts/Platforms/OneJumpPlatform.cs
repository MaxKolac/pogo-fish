using UnityEngine;

public class OneJumpPlatform : Platform
{
    void Update()
    {
        CheckPosition();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null || !collision.collider.CompareTag("Player")) return;
        if (collision.collider.transform.position.y > transform.position.y)
            Actions.OnPlatformDespawn?.Invoke(this, gameObject);
    }
}
