using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour, IDataPersistence
{
    [SerializeField] TMP_Text scoreText;
    public int CurrentScore { private set; get; } = 0;
    public int Highscore { private set; get; } = 0;

    private void OnEnable()
    {
        Actions.OnDeltaHeightChanged += IncreaseScore;
        Actions.OnGameLost += SetHighscore;
        CurrentScore = 0;
    }

    void OnDisable()
    {
        Actions.OnDeltaHeightChanged -= IncreaseScore;
        Actions.OnGameLost -= SetHighscore;
    }

    private void IncreaseScore(float deltaHeight) => CurrentScore += Mathf.RoundToInt(deltaHeight * 10f);
    private void SetHighscore() => Highscore = Mathf.Max(Highscore, CurrentScore);

    private void Update() => scoreText.text = "Score: " + CurrentScore;

    public void LoadData(GameData data) => Highscore = data.highscore;
    public void SaveData(ref GameData data) => data.highscore = Mathf.Max(data.highscore, CurrentScore);
}
