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

public class InGameUI : MonoBehaviour {
    public Text score;
    public Text highScore;
    public Image topBG;
    public Image pause;
    public ShapeShow nextShape;
    public Text PlayerMoneyText;
    public Text CountdownText;

    public CanvasGroup InGameUICanvas;
    private Sequence mScoreSequence;
    private void Start()
    {
        PlayerMoneyText.text = Managers.Game.stats.playerMoney.ToString();
        CountdownText.text = (Managers.Config.maxCountdown - 1).ToString();
        Managers.Game.stats.OnMoneyChanged += OnMoneyChanged;

        mScoreSequence = DOTween.Sequence();
        mScoreSequence.SetAutoKill(false);
    }

    private void OnDestroy()
    {
        Managers.Game.stats.OnMoneyChanged -= OnMoneyChanged;
    }

    void OnMoneyChanged(int changeMoney)
    {
        int playerMoney = Managers.Game.stats.playerMoney- changeMoney;
        mScoreSequence.Append(DOTween.To(delegate (float value) {
            //向下取整
            var temp = Mathf.FloorToInt(value);
            //向Text组件赋值
            PlayerMoneyText.text = temp + "";
        }, playerMoney, Managers.Game.stats.playerMoney, 1f));
    }
    public void UpdateScoreUI()
	{
        score.text = Managers.Score.currentScore.ToString();
        highScore.text = Managers.Score.highScore.ToString();
        if (Screen.width == 1125 && Screen.height == 2436){
             GetComponent<RectTransform>().anchoredPosition3D=new Vector3(0,-1040,0);
             transform.localScale=new Vector3(0.9f,0.9f,0.9f);
        }
    }

    public void isShow(bool isShow) {
        if (isShow) {
            InGameUICanvas.alpha = 1;
            InGameUICanvas.blocksRaycasts = true;

            if (Managers.Config.isShowNextShape == true && Managers.Game.stats.nextShapeID >= 0)
            {
                nextShape.gameObject.SetActive(true);
                nextShape.UpdateAttribute(Managers.Game.stats.nextShapeID,Managers.Game.stats.nextShapeIsBmob);
            }
            else {
                nextShape.gameObject.SetActive(false);
            }

            CountdownText.gameObject.SetActive(Managers.Config.isShowMergeObstacle || Managers.Config.isShowNotToMergeObstacle);
        }
        else {
            InGameUICanvas.alpha = 0;
            InGameUICanvas.blocksRaycasts = false;
        }
    }
    public void OnClickBackButton()
    {
        Managers.Audio.Play(SoundType.UIClick);
        Managers.Game.SetState(typeof(MenuState));
    }

}
