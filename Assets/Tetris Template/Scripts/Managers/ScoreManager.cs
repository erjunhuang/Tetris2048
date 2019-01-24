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
using System.Collections;



public class ScoreManager : MonoBehaviour {

	public int currentScore=0;
	public int highScore;

    void Start()
    {
        if (!Managers.Game.isGameActive) {
            highScore = Managers.Game.stats.highScore;
        }
        Managers.UI.inGameUI.UpdateScoreUI();
    }

	public void OnScore(int scoreIncreaseAmount)
	{	
		currentScore += scoreIncreaseAmount;
        CheckHighScore();
        Managers.UI.inGameUI.UpdateScoreUI();
    }

    public void CheckHighScore()
    {
		if (Managers.Game.stats.highScore < currentScore)
        {	
			highScore = currentScore;
        }
    }

    public void ResetScore(bool isRevive)
    {
        if (isRevive == false) {
            currentScore = 0;
        }
        highScore = Managers.Game.stats.highScore;
        Managers.UI.inGameUI.UpdateScoreUI();
    }

}
