using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private MobAdManager adManager;
    [Header("Background Scroller")]
    [SerializeField] private float xScroll;
    [SerializeField] private float yScroll;
    [SerializeField] private UnityEngine.UI.RawImage backImage;
    [Header("Shop Tab Contents")]
    [SerializeField] private GameObject skins;
    [SerializeField] private GameObject upgrade;

    void Start()
    {
        if (DataPersistenceManager.Instance.HasGameData())
            DataPersistenceManager.Instance.LoadGame();
        else
            DataPersistenceManager.Instance.NewGame();
        Actions.OnSkinClicked += audioManager.PlayClick;
        Actions.OnUpgradeClicked += audioManager.PlayClick;
        SwitchToSkinsPage();
        adManager.CreateBannerView();
        adManager.LoadBannerAd();
    }

    void OnDisable()
    {
        adManager.DestroyBannerAd();
        Actions.OnSkinClicked -= audioManager.PlayClick;
        Actions.OnUpgradeClicked -= audioManager.PlayClick;
    }

    void Update()
    {
        backImage.uvRect = new Rect(backImage.uvRect.position + new Vector2(xScroll, yScroll) * Time.deltaTime, backImage.uvRect.size);
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

    public void ReturnToGameScene() => SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
}
