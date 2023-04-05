using UnityEngine;

/// <summary>
/// Base script class of all Platforms.
/// </summary>
public class Platform : MonoBehaviour
{
    public enum PlatformType
    {
        Default, OneJump, SideWaysMoving
    }

    public PlatformType type;

    void Update()
    {
        CheckPosition();
    }

    /// <summary>
    /// Checks if the <c>Platform</c> is below screen's lower edge, which means the proper <c>Actions</c> should be invoked.
    /// Include this method in all subclasses' <c>Update()</c> methods, if they are different.
    /// </summary>
    protected void CheckPosition()
    {
        if (transform.position.y < GlobalAttributes.LowerScreenEdge)
            Actions.OnPlatformDespawn?.Invoke(this, gameObject);
    }

    /// <summary>
    /// Checks if the provided <c>collider</c> is a Player and if the Player is currently above the Platform.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the passed <c>collider</c> is Player and they are above the <c>Platform</c>. <c>false</c> if otherwise.
    /// </returns>
    protected bool IsColliderPlayerAndAbove(Collision2D collision)
    {
        if (collision == null 
            || !collision.collider.CompareTag("Player") 
            || (collision.collider.transform.position.y <= transform.position.y))
            return false;
        return true;
    }
}
