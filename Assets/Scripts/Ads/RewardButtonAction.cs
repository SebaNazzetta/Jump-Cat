using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardButtonAction : MonoBehaviour
{
    private RewardedAdsButton _rewardedAds;
    private void Awake()
    {
        _rewardedAds =  FindObjectOfType<RewardedAdsButton>();
        if(_rewardedAds)
            _rewardedAds.showAdButton = GetComponentInChildren<Button>();
    }

    private void OnEnable() 
    {
        if(_rewardedAds)
            _rewardedAds.LoadAd();
    }
}
