using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    [Header("Shop Tab Contents")]
    [SerializeField] private GameObject skins;
    [SerializeField] private GameObject upgrade;

    void Start()
    {
        if (DataPersistenceManager.Instance.HasGameData())
            DataPersistenceManager.Instance.LoadGame();
        else
            DataPersistenceManager.Instance.NewGame();
        SwitchToSkinsPage();
    }

    public void SwitchToSkinsPage()
    {
        upgrade.SetActive(false);
        skins.SetActive(true);
    }

    public void SwitchToUpgradesPage()
    {
        upgrade.SetActive(true);
        skins.SetActive(false);
    }

    public void ReturnToGameScene() => SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);//SceneHelper.LoadScene("GameScene", false, true);
}
