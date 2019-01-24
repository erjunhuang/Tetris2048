using System;
using UnityEngine;
using GoogleMobileAds.Api;

public enum RewardType
{
	Classical = 0,
	AddMoney = 1,
	Revive = 2,
}


// Example script showing how to invoke the Google Mobile Ads Unity plugin.
public class GoogleMobileAdsManger : MonoBehaviour
{

    private string iosAppId = "ca-app-pub-8215549831026018~8965769234";
    private string iosBannerId = "ca-app-pub-8215549831026018/3058159758";
    private string iosInterstitialId = "ca-app-pub-8215549831026018/3866994338";
    private string iosRewardedId = "ca-app-pub-8215549831026018/8579021690";

    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardBasedVideoAd rewardBasedVideo;
    private static string outputMessage = string.Empty;

    public bool isRewardBasedVideoClosed = false;

	private RewardType rewardType = RewardType.Classical;

    public static string OutputMessage
    {
        set { outputMessage = value; }
    }

    public void Start()
    {
        MobileAds.SetiOSAppPauseOnBackground(true);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(iosAppId);

        // Get singleton reward based video ad reference.
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;

        // RewardBasedVideoAd is a singleton, so handlers should only be registered once.
        this.rewardBasedVideo.OnAdLoaded += this.HandleRewardBasedVideoLoaded;
        this.rewardBasedVideo.OnAdFailedToLoad += this.HandleRewardBasedVideoFailedToLoad;
        this.rewardBasedVideo.OnAdOpening += this.HandleRewardBasedVideoOpened;
        this.rewardBasedVideo.OnAdStarted += this.HandleRewardBasedVideoStarted;
        this.rewardBasedVideo.OnAdRewarded += this.HandleRewardBasedVideoRewarded;
        this.rewardBasedVideo.OnAdClosed += this.HandleRewardBasedVideoClosed;
        this.rewardBasedVideo.OnAdLeavingApplication += this.HandleRewardBasedVideoLeftApplication;

        RequestBanner();
        RequestInterstitial();

        InvokeRepeating("RequestRewardBasedVideo", 0, 15f);
    }

    // Returns an ad request with custom ad targeting.
    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator)
            .AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
            .AddKeyword("game")
            .SetGender(Gender.Male)
            .SetBirthday(new DateTime(1985, 1, 1))
            .TagForChildDirectedTreatment(false)
            .AddExtra("color_bg", "9B30FF")
            .Build();
    }

    private void RequestBanner()
    {
        // Clean up banner ad before creating a new one.
        if (this.bannerView != null)
        {
            this.bannerView.Destroy();
        }

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(iosBannerId, AdSize.SmartBanner, AdPosition.Bottom);

        //this.bannerView.SetPosition((Screen.width - (int)this.bannerView.GetWidthInPixels()) / 2, (Screen.height - (int)this.bannerView.GetHeightInPixels()) / 4);

        // Register for ad events.
        this.bannerView.OnAdLoaded += this.HandleAdLoaded;
        this.bannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
        this.bannerView.OnAdOpening += this.HandleAdOpened;
        this.bannerView.OnAdClosed += this.HandleAdClosed;
        this.bannerView.OnAdLeavingApplication += this.HandleAdLeftApplication;

        // Load a banner ad.
        this.bannerView.LoadAd(this.CreateAdRequest());
    }

    private void RequestInterstitial()
    {
        // Clean up interstitial ad before creating a new one.
        if (this.interstitial != null)
        {
            this.interstitial.Destroy();
        }

        // Create an interstitial.
        this.interstitial = new InterstitialAd(iosInterstitialId);

        // Register for ad events.
        this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
        this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
        this.interstitial.OnAdOpening += this.HandleInterstitialOpened;
        this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
        this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;

        // Load an interstitial ad.
        this.interstitial.LoadAd(this.CreateAdRequest());
    }

    private void RequestRewardBasedVideo()
    {
        if (this.rewardBasedVideo.IsLoaded()==false) {
            this.rewardBasedVideo.LoadAd(this.CreateAdRequest(), iosRewardedId);
        }
    }

    public void ShowInterstitial()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
        else
        {
            MonoBehaviour.print("Interstitial is not ready yet");
            RequestInterstitial();
        }
    }

	public void ShowRewardBasedVideo(RewardType rewardType)
    {	
		this.rewardType = rewardType;
        if (this.rewardBasedVideo.IsLoaded())
        {
            this.rewardBasedVideo.Show();
        }
        else
        {
            MonoBehaviour.print("Reward based video ad is not ready yet");
            RequestRewardBasedVideo();
        }
    }

    public bool isIsLoadedVideo() {
        RequestRewardBasedVideo();
        return this.rewardBasedVideo.IsLoaded();
    }
    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeftApplication event received");
    }

    #endregion

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialLoaded event received");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialOpened event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialClosed event received");
        RequestInterstitial();
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialLeftApplication event received");
    }

    #endregion

    #region RewardBasedVideo callback handlers

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
        Managers.UI.gameStartManger.IsShowVideo();
        Managers.UI.popUps.gameOverPopUp.GetComponent<GameOverPopUp>().IsShowVideo();
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
		 
		if (this.rewardType == RewardType.Revive) {
			if (Managers.Game.State.ToString () == "GameOverState") {
				if (isRewardBasedVideoClosed) {
					isRewardBasedVideoClosed = false;
					Managers.Game.ReviveGame ();
				}
			}
		} 
		Managers.UI.gameStartManger.IsShowVideo();
		Managers.UI.popUps.gameOverPopUp.GetComponent<GameOverPopUp>().IsShowVideo();
		RequestRewardBasedVideo();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
		if (this.rewardType == RewardType.Revive) {
			if (Managers.Game.State.ToString () == "GameOverState") {
				isRewardBasedVideoClosed = true;
			}
		} else if (this.rewardType == RewardType.AddMoney) {
			if (Managers.Game.State.ToString () == "MenuState") {
				if (Managers.Game.stats.watchVideoTime > 0) {

					if (Managers.Game.stats.watchVideoTime == 3) {
						DateTime DateStart = new DateTime (1970, 1, 1, 8, 0, 0);
						int Createtime = Convert.ToInt32 ((DateTime.Now - DateStart).TotalSeconds);
						Managers.Game.stats.startWatchVideoTime = Createtime;
					}

					Managers.Game.stats.updateMoney (30);
					Managers.Game.stats.watchVideoTime--;
					Managers.UI.gameStartManger.updateVideoStatus ();
				}
			}
		}
        
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }

    #endregion
}
