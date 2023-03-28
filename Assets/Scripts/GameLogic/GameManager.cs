using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject titleScreenRoot;
    [SerializeField] private GameObject ingameScreenRoot;
    
    [SerializeField] private GameObject ground;
    [SerializeField] private GameObject heightSimulator;
    [SerializeField] private GameObject platformManager;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject scoreCounter;

    public enum GameState { TitleScreen, Playing, Paused, GameOver }
    public GameState CurrentGameState { private set; get; }

    void OnEnable() => ShowTitleScreen();

    public void ShowTitleScreen()
    {
        CurrentGameState = GameState.TitleScreen;
        titleScreenRoot.SetActive(true);
        ingameScreenRoot.SetActive(false);

        ground.SetActive(true);
        heightSimulator.SetActive(false);
        platformManager.SetActive(false);
        player.SetActive(true);
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
        player.SetActive(true);
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
