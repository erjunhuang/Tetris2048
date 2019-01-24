using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameStartManger : MonoBehaviour {
    public Text GameModeText, watchVideoTimeText;
    public Text[] PlayerMoneyTexts;
    public CanvasGroup menuCanvas, startGameCanvas, gameModeUICanvas;
    public Button showVideoBtn;

    public GameObject restartButton;

    public GameModeItem [] gameModeItems;
    private Sequence mScoreSequence;
    // Use this for initialization
    void Start () {
        Managers.Audio.PlayGameMusic();
        foreach (Text text in PlayerMoneyTexts)
        {
            text.text = Managers.Game.stats.playerMoney.ToString();
        }
		UptateUIStatus ();
        updateVideoStatus();
        Managers.Game.stats.OnMoneyChanged += OnMoneyChanged;

        mScoreSequence = DOTween.Sequence();
        mScoreSequence.SetAutoKill(false);
    }
	//DebugAddMoney
	public void DebugAddMoney(){
	
		Managers.Game.stats.updateMoney (1000);
	}
    private void OnDestroy()
    {
        Managers.Game.stats.OnMoneyChanged -= OnMoneyChanged;
    }

    void OnMoneyChanged(int changeMoney)
    {
        foreach (Text text in PlayerMoneyTexts)
        {
            int playerMoney = Managers.Game.stats.playerMoney - changeMoney;
            mScoreSequence.Append(DOTween.To(delegate (float value) {
                //向下取整
                var temp = Mathf.FloorToInt(value);
                //向Text组件赋值
                text.text = temp + "";
            }, playerMoney, Managers.Game.stats.playerMoney, 1f));
        }
    }
	void UptateUIStatus(){
		GameModeText.text = Managers.Game.stats.currentGameMode.ToString();
	}
    public void OnClickGameModeButton()
    {
        UIToUI(startGameCanvas, gameModeUICanvas);
        Managers.Audio.Play(SoundType.UIClick);
    }

    public void OnClickContinueButton()
    {
        Managers.Game.ContinueGame();
    }

    public void OnClickRestartButton()
    {
        Managers.Game.RestartGame(Managers.Game.stats.currentGameMode);
    }

    public void OnClickSettingsButton()
    {
        Managers.Audio.Play(SoundType.UIClick);
        Managers.UI.popUps.ActivateSettingsPopUp();
    }

    public void OnClickPlayerStatsButton()
    {
        Managers.Audio.Play(SoundType.UIClick);
        Managers.UI.popUps.ActivatePlayerStatsPopUp();
    }
    public void OnClickBackBtton()
    {
        Managers.Audio.Play(SoundType.UIClick);
		UptateUIStatus ();
        UIToUI(gameModeUICanvas, startGameCanvas);
    }

    public void UpdateGameModeItemsStatue(GameMode gameMode)
    {
        foreach (GameModeItem item in gameModeItems) {
            if (gameMode == item.gameMode) {
                item.updateStauts(false);
            }
        }
    }
    //看广告
    public void IsShowVideo()
    {
        showVideoBtn.gameObject.SetActive(Managers.Ads.isIsLoadedVideo());
    }
 
    public void OnClickShowVideoButton()
    {
        Managers.Audio.Play(SoundType.UIClick);
		Managers.Ads.ShowRewardBasedVideo(RewardType.AddMoney);
    }

    public void updateVideoStatus()
    {
        if (Managers.Game.stats.startWatchVideoTime > 0) {
            DateTime DateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            int currntTime = Convert.ToInt32((DateTime.Now - DateStart).TotalSeconds);
            if (currntTime - Managers.Game.stats.startWatchVideoTime >= 86400)
            {
                Managers.Game.stats.watchVideoTime = 3;
            }
        }

        int watchVideoTime = Managers.Game.stats.watchVideoTime;
        watchVideoTimeText.text = watchVideoTime.ToString();
    }
    //方法
    void UIToUI(CanvasGroup startUI, CanvasGroup endUI)
    {
        DOTween.To(() => startUI.alpha, x => startUI.alpha = x, 0, 0.5f);
        startUI.blocksRaycasts = false;

        DOTween.To(() => endUI.alpha, x => endUI.alpha = x, 1, 0.5f);
        endUI.blocksRaycasts = true;
    }
    public void isShow(bool isShow) {
        if (isShow)
        {
            DOTween.To(() => menuCanvas.alpha, x => menuCanvas.alpha = x,1, 0.5f);
            menuCanvas.blocksRaycasts = true;
        }
        else {
            DOTween.To(() => menuCanvas.alpha, x => menuCanvas.alpha = x, 0, 0.5f);
            menuCanvas.blocksRaycasts = false;
        }
    }
}
