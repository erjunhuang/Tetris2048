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
using DG.Tweening;

public class CameraManager : MonoBehaviour {

	public Camera main;
    public float _mainMenuSize = 15;
    public float _inGameSize = 11.7f;

    public float _inGameSizeForIphoneX = 13.8f;

    [HideInInspector]
	public CameraShake shaker;

    private Tweener ZoomInAnim;
    private Tweener ZoomOutAnim;

    void Awake()
	{
		shaker = main.gameObject.GetComponent<CameraShake> ();
        if (Screen.width == 1125 && Screen.height == 2436)
        {
            _inGameSize = _inGameSizeForIphoneX;
        }
    }

    public void ZoomIn()
    {
        if (main.orthographicSize != _inGameSize)
        {
            float animTime = 1f;
            StopAllAnim();
            ZoomInAnim = main.DOOrthoSize(_inGameSize, animTime).SetEase(Ease.OutCubic).OnComplete(() =>
             {
                 StartCoroutine(StartGamePlay());
             });
        }
        else
        {
            startGame();
        }
    }

    public void ZoomOut()
    {   
        Managers.Input.enabled=false;
        float animTime = 1f;
        StopAllAnim();
        if (main.orthographicSize != _mainMenuSize)
            ZoomOutAnim = main.DOOrthoSize(_mainMenuSize, animTime).SetEase(Ease.OutCubic);

        ZoomOutAnim.OnComplete(delegate ()
        {
        });
    }

    void StopAllAnim() {
        if (ZoomInAnim != null)
        {
            ZoomInAnim.Kill();
        }

        if (ZoomOutAnim != null)
        {
            ZoomOutAnim.Kill();
        }
    }

    IEnumerator StartGamePlay()
    {
        yield return new WaitForEndOfFrame();
        startGame();
        yield break;
    }

    void startGame() {
        if (!Managers.Game.isGameActive)
        {
            Managers.Spawner.Spawn();
            Managers.Game.isGameActive = true;
        }
        Managers.Input.enabled = true;
    }

}
