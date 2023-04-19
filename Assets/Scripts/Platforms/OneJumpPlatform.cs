using UnityEngine;

public class OneJumpPlatform : Platform
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsColliderPlayerAndAbove(collision))
            Actions.OnPlatformDespawn?.Invoke(this, gameObject);
    }
}
