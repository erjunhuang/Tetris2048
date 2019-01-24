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
using System.Collections;

public class PopUp : MonoBehaviour
{
    public GameObject gameOverPopUp;
    public GameObject settingsPopUp;
    public GameObject playerStatsPopUp;
    public GameObject isQuitPopUp;
    public GameObject CommondPopUp;

    public void ActivateGameOverPopUp()
    {
        Managers.UI.panel.SetActive(true);
        gameOverPopUp.transform.parent.gameObject.SetActive(true);
        gameOverPopUp.SetActive(true);
        Managers.UI.activePopUp = gameOverPopUp;
    }

    public void ActivateSettingsPopUp()
    {
        Managers.UI.panel.SetActive(true);
        settingsPopUp.transform.parent.gameObject.SetActive(true);
        settingsPopUp.SetActive(true);
        Managers.UI.activePopUp = settingsPopUp;
    }

    public void ActivatePlayerStatsPopUp()
    {
        Managers.UI.panel.SetActive(true);
        playerStatsPopUp.transform.parent.gameObject.SetActive(true);
        playerStatsPopUp.SetActive(true);
        Managers.UI.activePopUp = playerStatsPopUp;
    }

    public void ActivateQuitPopUp()
    {
        Managers.UI.panel.SetActive(true);
        isQuitPopUp.transform.parent.gameObject.SetActive(true);
        isQuitPopUp.SetActive(true);
        Managers.UI.activePopUp = isQuitPopUp;
    }

    public void ActivateCommondPopUp(GameMode gameMode ,int money)
    {
        Managers.UI.panel.SetActive(true);
        CommondPopUp.transform.parent.gameObject.SetActive(true);
        CommondPopUp.SetActive(true);
        CommondPopUp.transform.GetComponent<CommondDialog>().show(gameMode, money);

        Managers.UI.activePopUp = CommondPopUp;
    }

    public void Close(GameObject gameObject) {
        Managers.Audio.Play(SoundType.UIClick);

        gameObject.SetActive(false);
        gameObject.transform.parent.gameObject.SetActive(false);
        Managers.UI.panel.SetActive(false);
        if (gameObject == Managers.UI.activePopUp)
        {
            Managers.UI.activePopUp = null;
        }
    }

}
 
