using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    public int currentScore { private set; get; } = 0;

    private void OnEnable()
    {
        Actions.OnDeltaHeightChanged += IncreaseScore;
        currentScore = 0;
    }

    void OnDisable()
    {
        Actions.OnDeltaHeightChanged -= IncreaseScore;
    }

    private void IncreaseScore(float deltaHeight)
    {
        currentScore += Mathf.RoundToInt(deltaHeight * 10f);
    }

    private void Update()
    {
        scoreText.text = "Score: " + currentScore;
    }
}
