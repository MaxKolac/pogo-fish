using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    public int CurrentScore { private set; get; } = 0;

    private void OnEnable()
    {
        Actions.OnDeltaHeightChanged += IncreaseScore;
        CurrentScore = 0;
    }

    void OnDisable()
    {
        Actions.OnDeltaHeightChanged -= IncreaseScore;
    }

    private void IncreaseScore(float deltaHeight)
    {
        CurrentScore += Mathf.RoundToInt(deltaHeight * 10f);
    }

    private void Update()
    {
        scoreText.text = "Score: " + CurrentScore;
    }
}
