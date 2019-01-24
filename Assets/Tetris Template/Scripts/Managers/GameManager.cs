//  /*********************************************************************************
//   *********************************************************************************
//   *********************************************************************************
//   * Produced by Skard Games										                  *
//   * Facebook: https://goo.gl/5YSrKw											      *
//   * Contact me: https://goo.gl/y5awt4								              *											
//   * Developed by Cavit Baturalp Gürdin: https://tr.linkedin.com/in/baturalpgurdin *
//   *********************************************************************************
//   *********************************************************************************
//   *********************************************************************************/

using System;
using UnityEngine;
using UnityEngine.UI;

 

public class GameManager : MonoBehaviour
{
	public bool isGameActive;
    public TetrisShape currentShape;
    public Transform blockHolder;
    public PlayerStats stats;

    public int[] randoms = new int[13];
    public int index = 0;
    public bool isDebug = false;

    private _StatesBase currentState;
    public _StatesBase State
    {
        get { return currentState; }
    }
 
    void Awake()
	{
        isGameActive = false;
        //测试
        //Screen.SetResolution(720, 1280, true);
        PlayerData.CreateForPlayInfo();
    }

    void Start()
    {
        SetState(typeof(MenuState));
    }

    public void isShowLianXiaoAnim(int count) {
        if (count >2 )
        {
            GameObject go = Instantiate(Resources.Load("lianxiao", typeof(GameObject)) as GameObject);
            go.transform.parent = GameObject.Find("UI").transform;
            go.GetComponent<AnimatorManger>().show(count);
        }
    }
    //Changes the current game state
    public void SetState(System.Type newStateType)
	{
		if (currentState != null)
		{
			currentState.OnDeactivate();
		}

		currentState = GetComponentInChildren(newStateType) as _StatesBase;
		if (currentState != null)
		{
			currentState.OnActivate();
		}
	}

    public void ReviveGame()
    {
        Managers.Audio.Play(SoundType.UIClick);

        Managers.Game.stats.gamePlayTime++;
        Managers.Game.stats.ObstacleIndex = 0;
        Managers.Grid.ClearBoard(true);
        Managers.UI.HideIfOutside(Managers.UI.activePopUp);
        Managers.Game.SetState(typeof(GamePlayState));
		Managers.Game.stats.currentGameTimeSpent = Managers.UI.popUps.gameOverPopUp.GetComponent<GameOverPopUp> ().currentGameTimeSpent;
		Managers.Game.stats.ObstacleIndex = Managers.UI.popUps.gameOverPopUp.GetComponent<GameOverPopUp> ().ObstacleIndex;
    }
    public void ContinueGame() {
        Managers.Audio.Play(SoundType.UIClick);

        Managers.Game.SetState(typeof(GamePlayState));
    }
    public void RestartGame(GameMode gameMode)
    {
        Managers.Game.stats.currentGameMode = gameMode;
        Managers.Config.updateGameModeStaus();

        Managers.Game.isGameActive = false;

        Managers.Audio.Play(SoundType.UIClick);

        Managers.Game.stats.gamePlayTime = 0;
        Managers.Game.stats.ObstacleIndex = 0;
        Managers.Grid.ClearBoard(false);
        Managers.UI.HideIfOutside(Managers.UI.activePopUp);
        Managers.Game.SetState(typeof(GamePlayState));
    }

    void Update()
	{
		if (currentState != null)
		{
			currentState.OnUpdate();
		}
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause == true) {
            PlayerData.instance.PlayInfoSave();
        }
    }

    private void OnApplicationQuit()
    {
        PlayerData.instance.PlayInfoSave();
    }

    public void OnConnected()
    {
        if (isGameActive) {
			Managers.Grid.ClearGrid();
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 11; x++)
                {
                    if (Managers.Game.stats.gameGridcolForConnect[x].row[y] >= 0)
                    {

                        int idColor = Managers.Game.stats.gameGridcolForConnect[x].row[y];
                        Transform temp = CreateBlock(idColor);
                        temp.transform.position = new Vector3(x, y, 0);
                        Managers.Grid.gameGridcol[x].row[y] = temp;
                    }
                }
            }

            int currentShapeIdColor = Managers.Game.stats.currentShapeIdColor;
            if (currentShapeIdColor >= 0)
            {
                Managers.Game.currentShape = CreateBlock(currentShapeIdColor).GetComponent<TetrisShape>();
                Managers.Input.isActive = true;
            }
            Managers.UI.inGameUI.UpdateScoreUI();
        }

        Managers.UI.popUps.settingsPopUp.GetComponent<SettingsMenu>().syncMusic();
        Managers.Config.updateGameModeStaus();
    }

    Transform CreateBlock(int idColor)
    {
        bool isNotToMergeObstacle = false;
        if (idColor == Managers.Config.NotToMergeObstacleScore) {
            isNotToMergeObstacle = true;
        }
        TetrisShape tetrisShape = Managers.Spawner.InstantiateBlock(false,isNotToMergeObstacle, false);
        if (isNotToMergeObstacle)
        {
            tetrisShape.UpdateAttribute(idColor, BlockType.NotToMergeObstacle);
        }
        else {
            tetrisShape.UpdateAttribute(idColor, BlockType.Classical);
        }
         
        return tetrisShape.transform;
    }


}