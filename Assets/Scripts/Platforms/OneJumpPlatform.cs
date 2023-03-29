using UnityEngine;

public class OneJumpPlatform : Platform
{
    void Update()
    {
        CheckPosition();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            Actions.OnPlatformDespawn?.Invoke(this, gameObject);
    }
}
