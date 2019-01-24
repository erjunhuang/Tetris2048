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
using UnityEngine.UI;
using DG.Tweening;

public enum Menus
{
	MAIN,
	INGAME,
	GAMEOVER
}

public class UIManager : MonoBehaviour {

    public GameStartManger gameStartManger;
	public InGameUI inGameUI;
    public PopUp popUps;
    public GameObject activePopUp;
    public GameObject panel;

    public CanvasGroup gameCanvs;
    public  void Awake(){

    }

	public void ActivateUI(Menus menutype)
	{
		if (menutype.Equals (Menus.MAIN))
		{
            //gameCanvs.alpha = 0;
            DOTween.To(() => gameCanvs.alpha, x => gameCanvs.alpha = x, 0, 0.5f);
            gameCanvs.blocksRaycasts = false;

            gameStartManger.isShow(true);
            inGameUI.isShow(false);
        }
		else if(menutype.Equals(Menus.INGAME))
		{
            //gameCanvs.alpha = 1;
            DOTween.To(() => gameCanvs.alpha, x => gameCanvs.alpha = x, 1, 0.5f);
            gameCanvs.blocksRaycasts = true;

            gameStartManger.isShow(false);
            inGameUI.isShow(true);
        }	
	}

    void Update()
    {
        if (activePopUp != null&& activePopUp!= popUps.gameOverPopUp)
            HideIfClickedOutside(activePopUp);
    }

    public void MainMenuArrange()
    {
        if (Managers.Game.isGameActive)
        {
            gameStartManger.restartButton.SetActive(true);
        }
        else
        {
            gameStartManger.restartButton.SetActive(false);
        }
    }

    private void HideIfClickedOutside(GameObject outsidePanel)
    {
        if (Input.GetMouseButton(0) && outsidePanel.activeSelf &&
            !RectTransformUtility.RectangleContainsScreenPoint(
                outsidePanel.GetComponent<RectTransform>(),
                Input.mousePosition,
                Camera.main))
        {
            outsidePanel.SetActive(false);
            outsidePanel.transform.parent.gameObject.SetActive(false);
            Managers.UI.panel.SetActive(false);
            activePopUp = null;
        }
    }

    public void HideIfOutside(GameObject outsidePanel)
    {
        if (outsidePanel == null) return;

        outsidePanel.SetActive(false);
        outsidePanel.transform.parent.gameObject.SetActive(false);
        Managers.UI.panel.SetActive(false);
        if (outsidePanel == activePopUp)
        {
            activePopUp = null;
        }
    }

}
