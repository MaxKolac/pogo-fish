using UnityEngine;

public class GameOverScreenScript : MonoBehaviour
{
    [SerializeField] private ScoreCounter scoreCounterScript;
    [SerializeField] private TMPro.TMP_Text finalScoreText;

    void OnEnable()
    {
        finalScoreText.text = $"Game Over!\nFinal Score: {scoreCounterScript.currentScore}";
    }
}
