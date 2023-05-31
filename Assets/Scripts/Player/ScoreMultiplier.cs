﻿using System.Collections;
using UnityEngine;

public class ScoreMultiplier : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScoreCounter scoreCounterScript;
    [SerializeField] private UpgradeDurationBar upgradeDurationBar;
    [Header("Debugging")]
    [SerializeField] private float upgradeTimeLeft;

    public bool CoroutineRunning { get; private set; } = false;
    public bool CoroutinePaused { get; private set; } = false;

    private void OnEnable()
    {
        Actions.OnGameLost += StopPrematurily;
    }

    private void OnDisable()
    {
        Actions.OnGameLost -= StopPrematurily;
    }

    public void ActivateFor(float seconds)
    {
        gameObject.SetActive(true);
        CoroutineRunning = true;
        Actions.OnGamePaused += Pause;
        Actions.OnGameUnpaused += Unpause;

        scoreCounterScript.ScoreMultiplier = 2;
        upgradeTimeLeft = seconds;
        StartCoroutine(ScoreMultiplierCoroutine());
        upgradeDurationBar.ActivateBarFor(seconds);
    }

    public void StopPrematurily()
    {
        StopAllCoroutines();
        upgradeDurationBar.StopBarPrematurily();
        Decomission();
    }

    public void Pause()
    {
        if (CoroutineRunning)
            CoroutinePaused = true;
    }

    public void Unpause()
    {
        if (CoroutineRunning && CoroutinePaused)
            CoroutinePaused = false;
    }

    private void Decomission()
    {
        Actions.OnGamePaused -= Pause;
        Actions.OnGameUnpaused -= Unpause;
        upgradeTimeLeft = 0f;
        scoreCounterScript.ScoreMultiplier = 1;

        CoroutineRunning = false;
        gameObject.SetActive(false);
    }

    private IEnumerator ScoreMultiplierCoroutine()
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

    public void SetDurationTo(float seconds)
    {
        if (!CoroutineRunning) return;
        //Debug.Log($"Magnet duration reset from {upgradeTimeLeft} to {seconds}");
        upgradeDurationBar.SetTimeLeft(seconds);
        upgradeTimeLeft = seconds;
    }
}
