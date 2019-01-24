using UnityEngine;
using System.Collections;

public class GamePlayState : _StatesBase {

    public bool isShowObstacle = false;
    #region implemented abstract members of _StatesBase
    public override void OnActivate ()
	{
        Managers.UI.ActivateUI(Menus.INGAME);
        if (!Managers.Game.isGameActive) {
            Managers.Game.stats.currentGameTimeSpent = 0;
        }
        Managers.Cam.ZoomIn();

        //Debug.Log ("<color=green>Gameplay State</color> OnActive");
        
    }
	public override void OnDeactivate ()
	{
        //Debug.Log ("<color=red>Gameplay State</color> OnDeactivate");
    }

	public override void OnUpdate ()
    {    
        // Debug.Log ("<color=yellow>Gameplay State</color> OnUpdate");
        if (Managers.Game.currentShape != null) {
            Managers.Game.currentShape.movementController.ShapeUpdate();
        }

        Managers.Game.stats.currentGameTimeSpent += Time.deltaTime;
        
        if (Managers.Config.isShowMergeObstacle || Managers.Config.isShowNotToMergeObstacle)
        {
            float currentLoopTime = Mathf.Floor(Managers.Game.stats.currentGameTimeSpent % Managers.Config.maxCountdown);
            float showTime = Managers.Config.maxCountdown - 1;

            if (currentLoopTime == showTime&& isShowObstacle == false) {
                isShowObstacle = true;
                if (Managers.Config.isShowMergeObstacle && Managers.Config.isShowNotToMergeObstacle) {
                    if (UnityEngine.Random.Range(0, 5) <= 1)
                    {
                        Managers.Spawner.InstantiateNotToMergeObstacle(Managers.Grid.getCurrentTheHighest());
                    }
                    else {

                        Managers.Spawner.InstantiateMergeObstacle(Managers.Grid.getCurrentTheHighest());
                    }
                }
                else if (Managers.Config.isShowMergeObstacle)
                {
                    Managers.Spawner.InstantiateMergeObstacle(Managers.Grid.getCurrentTheHighest());
                }
                else if (Managers.Config.isShowNotToMergeObstacle)
                {
                    Managers.Spawner.InstantiateNotToMergeObstacle(Managers.Grid.getCurrentTheHighest());
                }
                
            }

            if (currentLoopTime != showTime)
            {
                isShowObstacle = false;
            }

            Managers.UI.inGameUI.CountdownText.text = (showTime - currentLoopTime).ToString();
        }
        
    }
	#endregion

}
