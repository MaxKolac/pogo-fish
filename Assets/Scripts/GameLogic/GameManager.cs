using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject titleScreenRoot;
    [SerializeField] private GameObject ingameScreenRoot;
    [SerializeField] private GameObject gameOverScreenRoot;
    
    [SerializeField] private GameObject ground;
    [SerializeField] private GameObject heightSimulator;
    [SerializeField] private PlatformManager platformManagerScript;
    [SerializeField] private Player playerScript;
    [SerializeField] private GameObject scoreCounter;

    public enum GameState { TitleScreen, Playing, Paused, GameOver }
    public static GameState CurrentGameState { private set; get; }

    void Start()
    {
        Actions.OnGameLost += EndGame;
        ShowTitleScreen();
    }

    public void ShowTitleScreen()
    {
        CurrentGameState = GameState.TitleScreen;
        titleScreenRoot.SetActive(true);
        ingameScreenRoot.SetActive(false);
        gameOverScreenRoot.SetActive(false);

        ground.SetActive(true);
        ground.transform.position = new Vector2(0f, 0.5f);
        heightSimulator.SetActive(false);
        playerScript.gameObject.SetActive(true);
        playerScript.Freeze();
        scoreCounter.SetActive(false);
        
    }

    public void BeginNewGame() 
    {
        CurrentGameState = GameState.Playing;
        titleScreenRoot.SetActive(false);
        ingameScreenRoot.SetActive(true);
        gameOverScreenRoot.SetActive(false);

        heightSimulator.SetActive(true);
        platformManagerScript.EnablePlatformSpawning();
        playerScript.Unfreeze();
        scoreCounter.SetActive(true);
    }

    public void PauseGame()
    {
        CurrentGameState = GameState.Paused;
    }

    public void EndGame()
    {
        CurrentGameState = GameState.GameOver;
        titleScreenRoot.SetActive(false);
        ingameScreenRoot.SetActive(false);
        gameOverScreenRoot.SetActive(true);

        heightSimulator.SetActive(false);
        platformManagerScript.DisablePlatformSpawning();
        playerScript.gameObject.SetActive(false);
        playerScript.ResetToStartingPosition();
    }
}
