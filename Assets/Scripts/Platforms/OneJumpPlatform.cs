using UnityEngine;

public class OneJumpPlatform : Platform
{
    void Start()
    {
        type = PlatformType.OneJump;    
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            Actions.OnPlatformDespawn?.Invoke(type, gameObject);
    }
}
