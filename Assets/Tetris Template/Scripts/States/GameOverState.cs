using UnityEngine;
using System.Collections;

public class GameOverState : _StatesBase {

	#region implemented abstract members of _StatesBase
	public override void OnActivate ()
	{
        //Debug.Log("<color=green>Game Over State</color> OnActive");
        Managers.Game.isGameActive = false;
        Managers.UI.popUps.ActivateGameOverPopUp();
        Managers.Audio.Play(SoundType.Lose);
        Managers.Input.enabled=false;

		Managers.Game.stats.updateGameData();

		//播放看全屏广告
		Managers.Ads.ShowInterstitial();
    }
  
    public override void OnDeactivate ()
    {
        //Managers.Adv.ShowRewardedAd();
        //Debug.Log ("<color=red>Game Over State</color> OnDeactivate");
    }

	public override void OnUpdate ()
	{
		// Debug.Log ("<color=yellow>Game Over State</color> OnUpdate");
	}
	#endregion

}
