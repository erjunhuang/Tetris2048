using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeItem : MonoBehaviour {
    public bool isLock = false;
    private Image image;
    private Button button;
    public GameMode gameMode;
    public int money;
	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickSelectGameModeBtton);

        isLock = Managers.Game.stats.gameModeIsLock[(int)gameMode];
        updateStauts(isLock);
    }

    public void updateStauts(bool isLock) {
        this.isLock = isLock;
        if (isLock)
        {
            image.sprite = Resources.Load("BtnType3", typeof(Sprite)) as Sprite;
        }
        else
        {
            image.sprite = Resources.Load("BtnType1", typeof(Sprite)) as Sprite;
        }
    }

    public void OnClickSelectGameModeBtton()
    {
        Managers.Audio.Play(SoundType.UIClick);
        if (isLock) {
            Managers.UI.popUps.ActivateCommondPopUp(gameMode, money);
        }
        else {
            //进入游戏
           Managers.Game.RestartGame(gameMode);
		   Managers.UI.gameStartManger.OnClickBackBtton ();
         }
    }
}
