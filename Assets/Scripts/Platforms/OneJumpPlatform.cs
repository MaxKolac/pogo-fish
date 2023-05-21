using UnityEngine;

public class OneJumpPlatform : Platform
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsCollidingWithPlayer(collision, true, true))
            Actions.OnPlatformDespawn?.Invoke(Type, gameObject);
    }
}
