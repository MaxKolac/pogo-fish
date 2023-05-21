using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { TitleScreen, Playing, Paused, GameOver }

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject titleScreenRoot;
    [SerializeField] private GameObject ingameScreenRoot;
    [SerializeField] private GameObject pauseScreenRoot;
    [SerializeField] private GameObject gameOverScreenRoot;
    
    [SerializeField] private GameObject ground;
    [SerializeField] private HeightSimulator heightSimulatorScript;
    [SerializeField] private PlatformManager platformManagerScript;
    [SerializeField] private Player playerScript;
    [SerializeField] private GameObject scoreCounter;

    public static GameState CurrentGameState { private set; get; }

    private Vector2 playerVelocityBeforePause;
    private float simulatorVelocityBeforePause;

    void Start()
    {
        Actions.OnGameLost += EndGame;
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
    }

    public void EndGame()
    {
        CurrentGameState = GameState.GameOver;
        titleScreenRoot.SetActive(false);
        ingameScreenRoot.SetActive(false);
        pauseScreenRoot.SetActive(false);
        gameOverScreenRoot.SetActive(true);

        heightSimulatorScript.gameObject.SetActive(false);
        platformManagerScript.DisablePlatformSpawning();
        playerScript.gameObject.SetActive(false);
        playerScript.ResetToStartingPosition();
    }

    public void LoadShopScene() => SceneHelper.LoadScene("ShopScene", false, true);
}
