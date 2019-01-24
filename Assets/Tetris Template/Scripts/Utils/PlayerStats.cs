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

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class ColumnForConnect
{
    public int[] row = new int[20];
}

public enum GameMode
{
    ClassicalMode = 0,
    AccelerationMode = 1,
    ObstacleMode = 2,
    ChallengeMode = 3,
}

public class PlayerStats: ScriptableObject
{   
    //游戏信息
    public int highScore = 0;
    public int totalScore = 0;
    public float timeSpent = 0;
    public int numberOfGames = 0;

    public float currentGameTimeSpent = 0;
    public bool isOpenMusic = true;
    public int gamePlayTime = 0;
     
    public int nextShapeID = -1;
    public bool nextShapeIsBmob = false;

    public int currentShapeIdColor = -1;

    public ColumnForConnect[] gameGridcolForConnect = new ColumnForConnect[11];
    public bool[] gameModeIsLock = new bool[4] {false,true,true,true};

    public GameMode currentGameMode = GameMode.ClassicalMode;

    public int playerMoney = 0;

    public delegate void OnMoneyChangedEvent(int changeMoney);
    public event OnMoneyChangedEvent OnMoneyChanged;
    public void updateMoney(int addMoney)
    {
        playerMoney = playerMoney + addMoney;
        OnMoneyChanged(addMoney);
    }

    public int ObstacleIndex = 0;

    public int watchVideoTime = 3;
    public int startWatchVideoTime = 0;
    

    public void sncyGridData() {
        //清空保持的数据
        Managers.Game.stats.CrearGrid();
        //加进去
        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 11; x++)
            {
                if (Managers.Grid.gameGridcol[x].row[y] != null)
                {
                    Transform oldObj = Managers.Grid.gameGridcol[x].row[y];

                    Managers.Game.stats.gameGridcolForConnect[x].row[y] = oldObj.GetComponent<TetrisShape>().idColor;
                }
            }
        }
    }

	public void updateGameData()
    {
        highScore = Managers.Score.highScore;
        totalScore += Managers.Score.currentScore;
        timeSpent += currentGameTimeSpent;
        numberOfGames++;

		currentGameTimeSpent = 0;
		nextShapeID = -1;
		Managers.UI.inGameUI.nextShape.gameObject.SetActive(false);
		currentShapeIdColor = -1;
		CrearGrid();
		ObstacleIndex = 0;
    }

    public void ClearStats()
    {
        highScore = 0;
        totalScore = 0;
        timeSpent = 0f;
        numberOfGames = 0;
    }

    public void CrearGrid()
    {
        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 11; x++)
            {
                Managers.Game.stats.gameGridcolForConnect[x].row[y] = -1;
            }
        }
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/PlayerStatistics")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<PlayerStats> ();
	}
#endif
}
