using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CommondDialog : MonoBehaviour {
    public Text showText;
    public Button OKButton, CancleButton;
    private GameMode gameMode;
    private int money;
    // Use this for initialization
    void Start () {

        OKButton.onClick.AddListener(OnOKButtonClick);
        CancleButton.onClick.AddListener(Close);
    }
    public void show(GameMode gameMode,int money) {
        showText.text = "do you want to unlock "+ gameMode.ToString()+" for " + money + " coin?";
        OKButton.interactable = Managers.Game.stats.playerMoney >= money;
        this.gameMode = gameMode;
        this.money = money;
     }
    void OnOKButtonClick()
    {
        Managers.Game.stats.updateMoney(-this.money);
        Managers.UI.gameStartManger.UpdateGameModeItemsStatue(gameMode);
        Managers.Game.stats.gameModeIsLock[(int)gameMode] = false;
        Close();
    }

    void Close() {
        Managers.UI.popUps.Close(this.gameObject);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
