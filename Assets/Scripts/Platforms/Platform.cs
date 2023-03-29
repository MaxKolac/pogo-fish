using UnityEngine;

/// <summary>
/// Base script class of all Platforms.
/// </summary>
public class Platform : MonoBehaviour
{
    public enum PlatformType
    {
        Default, OneJump, TwoJump
    }

    public PlatformType type = PlatformType.Default;

    void Update()
    {
        CheckPosition();
    }

    /// <summary>
    /// Checks if the <c>Platform</c> is below screen's lower edge, which means the proper <c>Actions</c> should be invoked.
    /// Include this method in all subclasses' <c>Update()</c> methods, if they are different.
    /// </summary>
    private void CheckPosition()
    {
        if (transform.position.y < GlobalAttributes.LowerScreenEdge)
            Actions.OnPlatformDespawn?.Invoke(type, gameObject);
    }
}
