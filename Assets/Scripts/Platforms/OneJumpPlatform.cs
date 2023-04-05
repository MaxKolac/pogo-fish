using UnityEngine;

public class OneJumpPlatform : Platform
{
    void Update()
    {
        CheckPosition();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsColliderPlayerAndAbove(collision))
            Actions.OnPlatformDespawn?.Invoke(this, gameObject);
    }
}
