using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class MobAdManager : MonoBehaviour
{
    #if UNITY_ANDROID
    private readonly string _adUnitId = "ca-app-pub-3940256099942544/6300978111";
    #elif UNITY_IPHONE
    private readonly string _adUnitId = "ca-app-pub-3940256099942544/2934735716";
    #else
    private readonly string _adUnitId = "unused";
    #endif

    public BannerView AdBanner { get; private set; }

    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");
        AdBanner = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Bottom);
    }

    /// <summary>
    /// Creates the banner view and loads a banner ad.
    /// </summary>
    public void LoadBannerAd()
    {
        // create an instance of a banner view first.
        if (AdBanner == null)
        {
            CreateBannerView();
        }
        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        AdBanner.LoadAd(adRequest);
    }

    /// <summary>
    /// Destroys the ad.
    /// </summary>
    public void DestroyBannerAd()
    {
        if (AdBanner != null)
        {
            Debug.Log("Destroying banner ad.");
            AdBanner.Destroy();
            AdBanner = null;
        }
    }

}
