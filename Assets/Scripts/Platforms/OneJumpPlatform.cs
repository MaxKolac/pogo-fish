using UnityEngine;

public class OneJumpPlatform : Platform
{
    public Vector2 DespawnPosition { get; private set; }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsCollidingWithPlayer(collision, true, true))
        {
            DespawnedByPlayer = true;
            DespawnPosition = transform.position;
            Actions.OnPlatformDespawn?.Invoke(this, gameObject);
        }
    }
}
