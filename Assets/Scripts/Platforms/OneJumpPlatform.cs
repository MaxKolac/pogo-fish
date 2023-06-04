using UnityEngine;

public class OneJumpPlatform : Platform
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsCollidingWithPlayer(collision, true, true))
        {
            DespawnedByPlayer = true;
            Actions.OnPlatformDespawn?.Invoke(this, gameObject);
        }
    }
}
