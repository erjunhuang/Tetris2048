using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameConfig : MonoBehaviour {

    public int  deleteID = 10;
    public bool isShowNextShape=true ;
    public bool isAccelerationTime=false;
    public bool isDestroyAround = true;
    public bool isShowMergeObstacle = false;
    public bool isShowNotToMergeObstacle = false;

    public int theHighest=20;
    public int theMostWide=11;
     
    public int startColorID=0;
    public int endColorID=6;

    public int maxCountdown = 30;

    public float minimumTime = 0.3f;

    public int NotToMergeObstacleScore = 3000;

    // Use this for initialization

    public void updateGameModeStaus()
    {
        theHighest = 15;
        theMostWide = 9;
        if (Managers.Game.stats.currentGameMode == GameMode.ClassicalMode)
        {
            isShowNextShape = true;
			isAccelerationTime = true;
			isDestroyAround = false;
            isShowMergeObstacle = false;
            isShowNotToMergeObstacle = false;
            startColorID = 0;
            endColorID = 6;
            deleteID = 10;
            maxCountdown = 30;
            minimumTime = 0.4f;
        }
        else if (Managers.Game.stats.currentGameMode == GameMode.AccelerationMode)
        {
            isShowNextShape = true;
            isAccelerationTime = true;
			isDestroyAround = true;
            isShowMergeObstacle = false;
            isShowNotToMergeObstacle = false;
            startColorID = 0;
            endColorID = 6;
            deleteID = 11;
            maxCountdown = 30;
            minimumTime = 0.4f;
        }
        else if (Managers.Game.stats.currentGameMode == GameMode.ObstacleMode)
        {
            isShowNextShape = true;
            isAccelerationTime = false;
            isDestroyAround = true;
            isShowMergeObstacle = true;
            isShowNotToMergeObstacle = false;
            startColorID = 0;
            endColorID = 8;
            deleteID = 10;
            maxCountdown = 15;
            minimumTime = 0.4f;
        }
        else if (Managers.Game.stats.currentGameMode == GameMode.ChallengeMode)
        {
            isShowNextShape = false;
            isAccelerationTime = true;
            isDestroyAround = true;
            isShowMergeObstacle = true;
            isShowNotToMergeObstacle = false;
            startColorID = 0;
            endColorID = 8;
            deleteID = 10;
            maxCountdown = 15;
            minimumTime = 0.4f;
        }

    }
     
    // Update is called once per frame
    void Update () {
        //if (Application.isLoadingLevel) {
        //    DontDestroyOnLoad(gameObject);
        //}
    }

}
