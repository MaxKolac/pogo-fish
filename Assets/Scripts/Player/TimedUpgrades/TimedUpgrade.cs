using System.Collections;
using UnityEngine;

public abstract class TimedUpgrade : MonoBehaviour
{
    [SerializeField] protected UpgradeBarsManager barsManager;
    [SerializeField] protected Sprite upgradeIcon;
    [Header("Debugging")]
    [SerializeField] protected float upgradeTimeLeft;
    [SerializeField] protected int reservedBarID;

    public bool CoroutineRunning { get; protected set; } = false;
    public bool CoroutinePaused { get; protected set; } = false;

    protected virtual void OnEnable() => Actions.OnGameLost += StopPrematurily;
    protected virtual void OnDisable() => Actions.OnGameLost -= StopPrematurily;

    /// <summary>
    /// Activates the TimedUpgrade and its effect by calling EnableEffect(). It also reserves a DurationBar.
    /// </summary>
    /// <param name="seconds">The amount of seconds to activate the TimedUpgrade for.</param>
    public void ActivateFor(float seconds)
    {
        gameObject.SetActive(true);
        CoroutineRunning = true;
        Actions.OnGamePaused += Pause;
        Actions.OnGameUnpaused += Unpause;

        EnableEffect();

        upgradeTimeLeft = seconds;
        StartCoroutine(EffectCoroutine());
        reservedBarID = barsManager.ReserveBar();
        barsManager.GetBarScript(reservedBarID).ActivateBarFor(seconds);
        barsManager.GetBarScript(reservedBarID).SetIconSprite(upgradeIcon);
    }

    /// <summary>
    /// Sets the duration of an already activated TimedUpgrade.
    /// </summary>
    /// <param name="seconds">The duration to set, in seconds.</param>
    public void SetDurationTo(float seconds)
    {
        if (!CoroutineRunning) return;
        //Debug.Log($"Magnet duration reset from {upgradeTimeLeft} to {seconds}");
        barsManager.GetBarScript(reservedBarID).SetTimeLeft(seconds);
        upgradeTimeLeft = seconds;
    }

    /// <summary>
    /// Method to call when the game is paused by user.
    /// </summary>
    public void Pause()
    {
        if (CoroutineRunning)
            CoroutinePaused = true;
    }

    /// <summary>
    /// Method to call when the game is unpaused by user.
    /// </summary>
    public void Unpause()
    {
        if (CoroutineRunning && CoroutinePaused)
            CoroutinePaused = false;
    }

    /// <summary>
    /// Method to call when the TimedUpgrade needs to be stopped before its duration runs out.
    /// </summary>
    public void StopPrematurily()
    {
        StopAllCoroutines();
        barsManager.GetBarScript(reservedBarID).StopBarPrematurily();
        Decomission();
    }

    /// <summary>
    /// Disables the TimedUpgrade and reverts all effects applied by it by calling DisableEffect(). Also, releases the related DurationBar.
    /// </summary>
    public void Decomission()
    {
        Actions.OnGamePaused -= Pause;
        Actions.OnGameUnpaused -= Unpause;
        upgradeTimeLeft = 0f;

        DisableEffect();

        barsManager.GetBarDictionaryEntry(reservedBarID).Release();
        CoroutineRunning = false;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Method called when method ActivateFor() is called.
    /// </summary>
    protected abstract void EnableEffect();
    /// <summary>
    /// Method called when method Decomission() is called.
    /// </summary>
    protected abstract void DisableEffect();

    /// <summary>
    /// A coroutine method which decreases upgradeTimeLeft every frame by Time.deltaTime.
    /// </summary>
    protected virtual IEnumerator EffectCoroutine()
    {
        while (true)
        {
            if (upgradeTimeLeft <= 0f)
            {
                Decomission();
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
            if (!CoroutinePaused)
                upgradeTimeLeft -= Time.deltaTime;
        }
    }
}
