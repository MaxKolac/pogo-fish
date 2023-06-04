using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour, IDataPersistence
{
    [Header("Settings References")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Text volumeText;
    [Header("Sounds Library")]
    [SerializeField] private List<AudioSource> audioSources;
    private readonly List<float> initialVolumes = new();
    public float CurrentVolume { get; private set; } = 1.0f;

    private void Start()
    {
        Actions.OnPickableObjectPickedUp += Play;
        Actions.OnPlatformDespawn += Play;
        InitializeVolumeList();
    }

    private void Update()
    {
        if (GameManager.CurrentGameState != GameState.Settings)
            return;
        CurrentVolume = volumeSlider.value;
        volumeText.text = "Volume: " + (CurrentVolume * 50f).ToString("N0") + "%";
        AdjustVolume(CurrentVolume);
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

    public void AdjustVolume(float volume)
    {
        CurrentVolume = volume;
        for (int i = 0; i < audioSources.Count; i++)
        {
            audioSources[i].volume = initialVolumes[i] * volume;
        }
    }

    private void InitializeVolumeList()
    {
        initialVolumes.Clear();
        foreach (AudioSource source in audioSources)
            initialVolumes.Add(source.volume);
    }

    public void LoadData(GameData data)
    {
        this.CurrentVolume = data.volume;
        if (volumeSlider != null)
            volumeSlider.value = data.volume;
        InitializeVolumeList();
        AdjustVolume(CurrentVolume);
    }

    public void SaveData(ref GameData data) => data.volume = this.CurrentVolume;
}
