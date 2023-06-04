using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class MobAdManager : MonoBehaviour
{
    [SerializeField] private ShopCoinCounter coinCounterScript;
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

    public void LoadAndShowInterstitialAd()
    {
        LoadRewardedInterstitialAd();
        ShowRewardedInterstitialAd();
        Actions.OnUpgradeClicked?.Invoke();
        Actions.OnSkinClicked?.Invoke();
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
                    Debug.LogError("rewarded interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                //Debug.Log("Rewarded interstitial ad loaded with response : "
                //          + ad.GetResponseInfo());

                rewardedInterstitialAd = ad;
            });
    }

    /// <summary>
    /// Shows the rewarded interstitial ad.
    /// </summary>
    public void ShowRewardedInterstitialAd()
    {
        //const string rewardMsg = "Rewarded interstitial ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedInterstitialAd != null && rewardedInterstitialAd.CanShowAd())
        {
            rewardedInterstitialAd.Show((Reward reward) =>
            {
                reward.Type = "Coins";
                reward.Amount = 50;
                //Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
            coinCounterScript.AddCoins(50);
        }
    }
}
