using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    public int currentScore { private set; get; } = 0;

    private void Start()
    {
        ResetScore();
    }

    public void ResetScore()
    {
        currentScore = 0;
    }

    public void IncreaseScore(float deltaHeight)
    {
        currentScore += Mathf.RoundToInt(deltaHeight * 10f);
    }

    private void Update()
    {
        scoreText.text = "Score: " + currentScore;
    }
}
