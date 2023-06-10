using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public enum GameState { TitleScreen, Playing, Paused, GameOver, Settings }

public class GameManager : MonoBehaviour
{
    [Header("UI Screen Roots")]
    [SerializeField] private GameObject titleScreenRoot;
    [SerializeField] private GameObject ingameScreenRoot;
    [SerializeField] private GameObject pauseScreenRoot;
    [SerializeField] private GameObject gameOverScreenRoot;
    [SerializeField] private GameObject settingsScreenRoot;
    [Header("GameObject Scripts")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private HeightSimulator heightSimulatorScript;
    [SerializeField] private PlatformManager platformManagerScript;
    [SerializeField] private Player playerScript;
    [SerializeField] private ScoreCounter scoreCounterScript;
    [Header("GameObjects")]
    [SerializeField] private GameObject ground;
    [SerializeField] private TMP_Text highscoreText;
    [SerializeField] private GameObject scoreCounter;
    [Header("Prefabs")]
    [SerializeField] private GameObject dataPersistenceManagerPrefab;
    [Header("Background Settings")]
    [SerializeField] private float xScroll;
    [SerializeField] private float yScroll;
    [SerializeField] private RawImage backgroundImage;

    public static GameState CurrentGameState { private set; get; }

    private Vector2 playerVelocityBeforePause;
    private float simulatorVelocityBeforePause;

    void Awake()
    {
        if (DataPersistenceManager.Instance == null)
        {
            GameObject dataPerManager = Instantiate(dataPersistenceManagerPrefab);
            dataPerManager.SetActive(true);
        }
    }

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

    void Update()
    {
        backgroundImage.uvRect = new Rect(backgroundImage.uvRect.position + new Vector2(xScroll, yScroll) * Time.deltaTime, backgroundImage.uvRect.size);
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
        settingsScreenRoot.SetActive(false);
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
        settingsScreenRoot.SetActive(false);

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
        settingsScreenRoot.SetActive(false);

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
        settingsScreenRoot.SetActive(false);

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
        settingsScreenRoot.SetActive(false);

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

    public void ShowSettings()
    {
        if (CurrentGameState != GameState.TitleScreen)
            return;
        CurrentGameState = GameState.Settings;
        settingsScreenRoot.SetActive(true);
        titleScreenRoot.SetActive(false);
        ingameScreenRoot.SetActive(false);
        pauseScreenRoot.SetActive(false);
        gameOverScreenRoot.SetActive(false);
    }

    public void HideSettings()
    {
        if (CurrentGameState != GameState.Settings)
            return;
        audioManager.AdjustVolume(audioManager.CurrentVolume);
        DataPersistenceManager.Instance.SaveGame();
        ShowTitleScreen();
    }

    public void EraseProgress()
    {
        if (CurrentGameState != GameState.Settings)
            return;
        DataPersistenceManager.Instance.EraseGame();
        ShowTitleScreen();
    }

    public void LoadShopScene() => SceneManager.LoadSceneAsync("ShopScene", LoadSceneMode.Single);
}
