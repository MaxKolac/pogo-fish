using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    void Start()
    {
        if (DataPersistenceManager.Instance.HasGameData())
            DataPersistenceManager.Instance.LoadGame();
        else
            DataPersistenceManager.Instance.NewGame();
    }

    public void SwitchToSkinsPage()
    {

    }

    public void SwitchToUpgradesPage()
    {

    }

    public void ReturnToGameScene() => SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);//SceneHelper.LoadScene("GameScene", false, true);
}
