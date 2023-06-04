using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> audioSources;

    private void Start()
    {
        Actions.OnPickableObjectPickedUp += Play;
        Actions.OnPlatformDespawn += Play;
    }

    private void OnDisable()
    {
        Actions.OnPickableObjectPickedUp -= Play;
        Actions.OnPlatformDespawn -= Play;
    }

    public void Play(string soundName)
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.clip.name == soundName)
            {
                source.Play();
                return;
            }
        }
        Debug.LogError($"AudioManager failed to find an AudioSource with clip.name of {soundName}!");
    }

    public void PlayClick() => Play("click");

    private void Play(PickableObject pickableObjScript, GameObject gameObj)
    {
        switch (pickableObjScript.Type)
        {
            case PickableObjectType.Coin:
                Play("pop");
                break;
            case PickableObjectType.SpringBoost:
                Play("spring");
                break;
            case PickableObjectType.Magnet:
                Play("magnet");
                break;
            case PickableObjectType.ScoreMultiplier:
                Play("scoreMultiplier");
                break;
            default:
                Debug.LogWarning("AudioManager doesn't recognized this PickableObject! No sound playing!");
                break;
        }
    }

    private void Play(Platform platformScript, GameObject gameObj)
    {
        switch (platformScript.Type)
        {
            case PlatformType.OneJump:
                if (platformScript.DespawnedByPlayer == true)
                {
                    if (Random.Range(0, 2) == 0)
                        Play("platform_break1");
                    else
                        Play("platform_break2");
                }
                break;
            case PlatformType.SideWaysMoving:
            case PlatformType.Default:
                //silence
                break;
            default:
                Debug.LogWarning("AudioManager doesn't recognized this Platform.Type! No sound playing!");
                break;
        }
    }
}
