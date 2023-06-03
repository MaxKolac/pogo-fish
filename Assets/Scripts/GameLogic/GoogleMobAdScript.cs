using System;
using UnityEngine;
using GoogleMobileAds.Api;

internal class GoogleMobAdScript : MonoBehaviour
{
    private void Start() => MobileAds.Initialize(initStatus => { });
}