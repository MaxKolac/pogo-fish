using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToActiveInOrder;

    public enum GameState { TitleScreen, Playing, Paused, GameOver }
    public GameState CurrentGameState { private set; get; }

    void Start()
    {
        CurrentGameState = GameState.TitleScreen;
        ActivateListedGameObjects();
        //Show TitleScreen UI
    }

    public void BeginNewGame() 
    {
        CurrentGameState = GameState.Playing;
        ActivateListedGameObjects();
    }

    public void EndGame()
    {
        CurrentGameState = GameState.GameOver;
    }

    void ActivateListedGameObjects()
    {
        foreach (GameObject gameObject in objectsToActiveInOrder)
            gameObject.SetActive(true);
    }
}
