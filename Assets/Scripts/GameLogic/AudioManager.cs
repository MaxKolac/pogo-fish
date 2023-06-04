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
                //Play("spring");
                break;
            case PickableObjectType.Magnet:
                Play("magnet");
                break;
            case PickableObjectType.ScoreMultiplier:
                //TODO
                break;
            default:
                Debug.LogWarning("AudioManager doesn't recognized this PickableObject! No sound playing!");
                break;
        }
    }

    private void Play(PlatformType type, GameObject gameObj)
    {
        switch (type)
        {
            case PlatformType.OneJump:
                break;
            case PlatformType.SideWaysMoving:
            case PlatformType.Default:
                //silence
                break;
            default:
                Debug.LogWarning("AudioManager doesn't recognized this PlatformType! No sound playing!");
                break;
        }
    }
}
