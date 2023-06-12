using System;
using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class MobAdManager : MonoBehaviour
{
    [SerializeField] private float rewardedAdTimeout;
    [Header("References")]
    [SerializeField] private ShopCoinCounter coinCounterScript;
    [SerializeField] private Button adButton;
    private readonly string bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111"; 
    private readonly string rewardedAdUnitId = "ca-app-pub-3940256099942544/5354046379";

    private BannerView bannerAd;
    private RewardedInterstitialAd rewardedInterstitialAd;

    public void CreateBannerView()
    {
        //Debug.Log("Creating banner view");
        bannerAd = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);
    }

    /// <summary>
    /// Creates the banner view and loads a banner ad.
    /// </summary>
    public void LoadBannerAd()
    {
        // create an instance of a banner view first.
        if (bannerAd == null)
        {
            CreateBannerView();
        }
        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        //Debug.Log("Loading banner ad.");
        bannerAd.LoadAd(adRequest);
    }

    /// <summary>
    /// Destroys the ad.
    /// </summary>
    public void DestroyBannerAd()
    {
        if (bannerAd != null)
        {
            //Debug.Log("Destroying banner ad.");
            bannerAd.Destroy();
            bannerAd = null;
        }
    }

    /// <summary>
    /// Loads the rewarded interstitial ad.
    /// </summary>
    public void LoadRewardedInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedInterstitialAd != null)
        {
            rewardedInterstitialAd.Destroy();
            rewardedInterstitialAd = null;
        }
        //Debug.Log("Loading the rewarded interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        RewardedInterstitialAd.Load(rewardedAdUnitId, adRequest,
            (RewardedInterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("rewardedInterstitialAd failed to load an ad with error: " + error);
                    return;
                }
                //Debug.Log("Rewarded interstitial ad loaded with response : " + ad.GetResponseInfo());
                rewardedInterstitialAd = ad;
            }
            );
    }

    /// <summary>
    /// Shows the rewarded interstitial ad.
    /// </summary>
    public void ShowRewardedInterstitialAd()
    {
        if (rewardedInterstitialAd != null && rewardedInterstitialAd.CanShowAd())
        {
            rewardedInterstitialAd.Show((Reward reward) =>
            {
                reward.Type = "Coins";
                reward.Amount = 50;
                coinCounterScript.AddCoins((int)reward.Amount);
                //Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
            Actions.OnUpgradeClicked?.Invoke();
            Actions.OnSkinClicked?.Invoke();
        }
    }

    private IEnumerator RewardedInterstitialCoroutine()
    {
        adButton.interactable = false;
        float timer = rewardedAdTimeout;
        LoadRewardedInterstitialAd();
        while (!rewardedInterstitialAd.CanShowAd())
        {
            if (timer <= 0)
            {
                adButton.interactable = true;
                Debug.LogWarning("Failed to show rewardedInterstitialAd. Time limit reached and ad still can't be shown. Aborting coroutine...");
                yield break;
            }
            timer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        adButton.interactable = true;
        ShowRewardedInterstitialAd();
    }

    public void StartAdCoroutine() => StartCoroutine(RewardedInterstitialCoroutine());
}
