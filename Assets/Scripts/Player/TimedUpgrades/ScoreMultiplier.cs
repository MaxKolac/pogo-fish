using UnityEngine;

public class ScoreMultiplier : TimedUpgrade
{
    [SerializeField] private ScoreCounter scoreCounterScript;

    protected override void EnableEffect() => scoreCounterScript.ScoreMultiplier = 2;
    protected override void DisableEffect() => scoreCounterScript.ScoreMultiplier = 1;
}
