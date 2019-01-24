//  /*********************************************************************************
//   *********************************************************************************
//   *********************************************************************************
//   * Produced by Skard Games										                 *
//   * Facebook: https://goo.gl/5YSrKw											     *
//   * Contact me: https://goo.gl/y5awt4								             *
//   * Developed by Cavit Baturalp Gürdin: https://tr.linkedin.com/in/baturalpgurdin *
//   *********************************************************************************
//   *********************************************************************************
//   *********************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GoogleMobileAds.Api;
using System;

public class GameOverPopUp : MonoBehaviour {

    public Text gameOverScore, AddMoney;
    public Button ReviveBtn,MoneyReviveBtn;
    public bool isRewardBasedVideoClosed;
    public int ReviveMoney = 100;

	public float currentGameTimeSpent = 0;
	public int ObstacleIndex=0;
    void OnEnable()
    {	
        gameOverScore.text = Managers.Score.currentScore.ToString();

        int money = Mathf.FloorToInt(Managers.Score.currentScore / 4000 + Managers.Game.stats.currentGameTimeSpent / 15);
        AddMoney.text = "Money:+" + money;
        Managers.Game.stats.updateMoney(money);

        //显示金币复活按钮
        if (Managers.Game.stats.gamePlayTime == 0 && Managers.Game.stats.playerMoney >= ReviveMoney)
        {
            MoneyReviveBtn.interactable = true;
        }
        else
        {
            MoneyReviveBtn.interactable = false;
        }

        IsShowVideo();
		currentGameTimeSpent = Managers.Game.stats.currentGameTimeSpent;
		ObstacleIndex = Managers.Game.stats.ObstacleIndex;
    }

    public void IsShowVideo() {
        // 显示看视频广告按钮
        if (Managers.Ads.isIsLoadedVideo() && Managers.Game.stats.gamePlayTime == 0)
        {
            ReviveBtn.interactable = true;
        }
        else
        {
            ReviveBtn.interactable = false;
        }
    }
    public void ShowRewardBasedVideo()
    {
		Managers.Ads.ShowRewardBasedVideo(RewardType.Revive);
    }

    public void BackToMainMenu()
    {
        Managers.Game.stats.gamePlayTime = 0;
        Managers.Grid.ClearBoard(false);
        Managers.Audio.Play(SoundType.UIClick);
        Managers.UI.HideIfOutside(this.gameObject);
        Managers.Game.isGameActive = false;
        Managers.Game.SetState(typeof(MenuState));
    }
     
    public void MoneyRevive() {
        if (Managers.Game.stats.playerMoney >= ReviveMoney) {
            Managers.Game.stats.updateMoney(-ReviveMoney);
            Managers.Game.ReviveGame();
        }
    }

     

    public void OnClickRestartButton()
    {
        Managers.Game.stats.gamePlayTime = 0;
        Managers.Game.stats.ObstacleIndex = 0;
        Managers.Audio.Play(SoundType.UIClick);
        Managers.Grid.ClearBoard(false);
        Managers.Game.isGameActive = false;
        Managers.Game.SetState(typeof(GamePlayState));
        Managers.UI.HideIfOutside(Managers.UI.popUps.gameOverPopUp);
    }
}
