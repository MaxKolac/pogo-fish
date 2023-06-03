using System.Collections;
using UnityEngine;

public class UpgradeDurationBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer progressBar;
    [SerializeField] private SpriteMask progressMask;
    [SerializeField] private SpriteRenderer upgradeIcon;
    [Header("Timers and Numbers")]
    [SerializeField] private float initialTime;
    [SerializeField] private float upgradeTimeLeft;
    [SerializeField] private float percentageLeft;
    private Vector2 initialMaskPosition;
    private float maskDistanceToBarPivot;

    public bool IsCountingDown { get; private set; }
    public bool IsPaused { get; private set; }

    private void OnEnable()
    {
        maskDistanceToBarPivot = 8.8f * transform.localScale.x;
        initialMaskPosition = progressMask.transform.position = transform.position + new Vector3(maskDistanceToBarPivot, 0f, 0f);
    }
    
    public void ActivateBarFor(float seconds)
    {
        IsCountingDown = true;
        gameObject.SetActive(true);
        Actions.OnGamePaused += Pause;
        Actions.OnGameUnpaused += Unpause;
        Actions.OnGameAbandoned += StopBarPrematurily;
        progressMask.transform.position = initialMaskPosition;
        initialTime = upgradeTimeLeft = seconds;
        percentageLeft = 1.0f; 
        progressBar.color = TranslatePercentageToColor(1.0f);
        StartCoroutine(UpgradeDurationBarCoroutine());
    }

    public void StopBarPrematurily()
    {
        StopAllCoroutines();
        Decomission();
    }

    public void Pause()
    {
        if (IsCountingDown)
            IsPaused = true;
    }

    public void Unpause()
    {
        if (IsCountingDown && IsPaused)
            IsPaused = false;
    }

    private void Decomission()
    {
        Actions.OnGamePaused -= Pause;
        Actions.OnGameUnpaused -= Unpause;
        Actions.OnGameAbandoned -= StopBarPrematurily;

        progressMask.transform.position = initialMaskPosition;
        gameObject.SetActive(false);
        IsCountingDown = false; 
        IsPaused = false;
        Actions.OnTimedUpgradeExpires?.Invoke();
    }

    public void SetTimeLeft(float seconds)
    {
        if (!IsCountingDown)
        {
            return;
        }
        if (seconds > initialTime)
        {
            initialTime = seconds;
        }
        upgradeTimeLeft = seconds;
        percentageLeft = seconds / initialTime;
    }

    public void SetIconSprite(Sprite icon) => upgradeIcon.sprite = icon;

    private IEnumerator UpgradeDurationBarCoroutine()
    {
        while (true)
        {
            //Change color of progressBar;
            progressBar.color = TranslatePercentageToColor(percentageLeft);
            //Clip (somehow) the progressBar;
            progressMask.transform.position = new Vector2
                (
                this.transform.position.x + (percentageLeft * maskDistanceToBarPivot),
                progressMask.transform.position.y
                );

            if (upgradeTimeLeft <= 0)
            {
                Decomission();
                yield break;
            }

            yield return new WaitForSeconds(Time.deltaTime);
            if (!IsPaused)
            {
                upgradeTimeLeft -= Time.deltaTime;
                percentageLeft = upgradeTimeLeft / initialTime;
            }
        }
    }

    private Color TranslatePercentageToColor(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);
        float red = 255f;
        float green = 255f;
        if (percentage >= 0.5f)
        {
            red = 255f * (2f * (1f - percentage));
        }
        else
        {
            green = 255f * (2f * percentage);
        }
        //Debug.Log($"Color ({red / 255f},{green / 255f},0)");
        return new Color(red / 255f, green / 255f, 0);
    }
}
