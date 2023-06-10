using UnityEngine;

public class OneJumpPlatform : Platform
{
    public Vector2 DespawnPosition { get; private set; }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsCollisionWithPlayerValid(collision))
        {
            DespawnedByPlayer = true;
            DespawnPosition = transform.position;
            Actions.OnPlatformDespawn?.Invoke(this, gameObject);
        }
    }
}
