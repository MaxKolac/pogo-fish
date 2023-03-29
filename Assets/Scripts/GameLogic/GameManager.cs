using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject titleScreenRoot;
    [SerializeField] private GameObject ingameScreenRoot;
    
    [SerializeField] private GameObject ground;
    [SerializeField] private GameObject heightSimulator;
    [SerializeField] private GameObject platformManager;
    [SerializeField] private Player playerScript;
    [SerializeField] private GameObject scoreCounter;

    public enum GameState { TitleScreen, Playing, Paused, GameOver }
    public static GameState CurrentGameState { private set; get; }

    void Start() => ShowTitleScreen();

    public void ShowTitleScreen()
    {
        CurrentGameState = GameState.TitleScreen;
        titleScreenRoot.SetActive(true);
        ingameScreenRoot.SetActive(false);

        ground.SetActive(true);
        heightSimulator.SetActive(false);
        platformManager.SetActive(false);
        playerScript.TitleScreenFreeze();
        scoreCounter.SetActive(false);
    }

    public void BeginNewGame() 
    {
        CurrentGameState = GameState.Playing;
        titleScreenRoot.SetActive(false);
        ingameScreenRoot.SetActive(true);

        ground.SetActive(true);
        heightSimulator.SetActive(true);
        platformManager.SetActive(true);
        playerScript.TitleScreenUnfreeze();
        scoreCounter.SetActive(true);
    }

    public void PauseGame()
    {
        CurrentGameState = GameState.Paused;
    }

    public void EndGame()
    {
        CurrentGameState = GameState.GameOver;
    }
}
