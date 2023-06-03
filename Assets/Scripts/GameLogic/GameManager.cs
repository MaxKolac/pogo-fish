using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public enum GameState { TitleScreen, Playing, Paused, GameOver }

public class GameManager : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioSource clickSound;
    [Header("UI Screen Roots")]
    [SerializeField] private GameObject titleScreenRoot;
    [SerializeField] private GameObject ingameScreenRoot;
    [SerializeField] private GameObject pauseScreenRoot;
    [SerializeField] private GameObject gameOverScreenRoot;
    [Header("GameObject Scripts")]
    [SerializeField] private HeightSimulator heightSimulatorScript;
    [SerializeField] private PlatformManager platformManagerScript;
    [SerializeField] private Player playerScript;
    [SerializeField] private ScoreCounter scoreCounterScript;
    [Header("GameObjects")]
    [SerializeField] private GameObject ground;
    [SerializeField] private TMP_Text highscoreText;
    [SerializeField] private GameObject scoreCounter;

    public static GameState CurrentGameState { private set; get; }

    private Vector2 playerVelocityBeforePause;
    private float simulatorVelocityBeforePause;

    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        Actions.OnGameLost += EndGame;
        if (DataPersistenceManager.Instance.HasGameData())
            DataPersistenceManager.Instance.LoadGame();
        else
            DataPersistenceManager.Instance.NewGame();
        ShowTitleScreen();
    }

    void OnDestroy()
    {
        Actions.OnGameLost -= EndGame;
    }

    public void ShowTitleScreen()
    {
        CurrentGameState = GameState.TitleScreen;
        titleScreenRoot.SetActive(true);
        ingameScreenRoot.SetActive(false);
        pauseScreenRoot.SetActive(false);
        gameOverScreenRoot.SetActive(false);
        highscoreText.text = $"Highscore: {scoreCounterScript.Highscore}";

        ground.SetActive(true);
        ground.transform.position = new Vector2(0f, 0.5f);
        heightSimulatorScript.gameObject.SetActive(false);
        playerScript.gameObject.SetActive(true);
        playerScript.Freeze();
        scoreCounter.SetActive(false);
    }

    public void BeginNewGame() 
    {
        CurrentGameState = GameState.Playing;
        titleScreenRoot.SetActive(false);
        ingameScreenRoot.SetActive(true);
        pauseScreenRoot.SetActive(false);
        gameOverScreenRoot.SetActive(false);

        heightSimulatorScript.gameObject.SetActive(true);
        platformManagerScript.EnablePlatformSpawning();
        playerScript.Unfreeze();
        scoreCounter.SetActive(true);
    }

    public void PauseGame()
    {
        CurrentGameState = GameState.Paused;
        titleScreenRoot.SetActive(false);
        ingameScreenRoot.SetActive(false);
        pauseScreenRoot.SetActive(true);
        gameOverScreenRoot.SetActive(false);

        playerVelocityBeforePause =
            heightSimulatorScript.IsFrozen ?
            playerScript.GetVelocity() :
            new Vector2(playerScript.GetVelocity().x, heightSimulatorScript.GetVerticalVelocity());
        playerScript.Freeze();
        simulatorVelocityBeforePause = heightSimulatorScript.GetVerticalVelocity();
        heightSimulatorScript.Freeze();
        Actions.OnGamePaused?.Invoke();
    }

    public void UnpauseGame()
    {
        CurrentGameState = GameState.Playing;
        titleScreenRoot.SetActive(false);
        ingameScreenRoot.SetActive(true);
        pauseScreenRoot.SetActive(false);
        gameOverScreenRoot.SetActive(false);

        playerScript.Unfreeze();
        playerScript.SetVelocity(playerVelocityBeforePause); 
        heightSimulatorScript.Unfreeze();
        heightSimulatorScript.SetVerticalVelocity(simulatorVelocityBeforePause);
        Actions.OnGameUnpaused?.Invoke();
    }

    public void EndGame()
    {
        CurrentGameState = GameState.GameOver;
        DataPersistenceManager.Instance.SaveGame();
        titleScreenRoot.SetActive(false);
        ingameScreenRoot.SetActive(false);
        pauseScreenRoot.SetActive(false);
        gameOverScreenRoot.SetActive(true);

        heightSimulatorScript.gameObject.SetActive(false);
        platformManagerScript.DisablePlatformSpawning();
        playerScript.gameObject.SetActive(false);
    }

    public void AbandonGame()
    {
        Actions.OnGameAbandoned?.Invoke();
        platformManagerScript.DisablePlatformSpawning();
        playerScript.gameObject.SetActive(false);
        ShowTitleScreen();
    }

    public void LoadShopScene() => SceneManager.LoadSceneAsync("ShopScene", LoadSceneMode.Single);
}
