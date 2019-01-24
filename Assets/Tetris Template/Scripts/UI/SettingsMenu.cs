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

public class SettingsMenu : MonoBehaviour
{
    public GameObject soundCross;
    public GameObject IsQuit;

    public void TurnUpDownSound()
    {
        if (AudioListener.volume == 0)
        {
            soundCross.SetActive(false);
            AudioListener.volume = 1.0f;
            Managers.Audio.Play(SoundType.UIClick);
            Managers.Game.stats.isOpenMusic = true;
        }
        else if (AudioListener.volume == 1.0f)
        {
            soundCross.SetActive(true);
            AudioListener.volume = 0f;
            Managers.Game.stats.isOpenMusic = false;
        }
    }

    public void syncMusic()
    {
        if (Managers.Game.stats.isOpenMusic)
        {
            soundCross.SetActive(false);
            AudioListener.volume = 1.0f;
        }
        else
        {
            soundCross.SetActive(true);
            AudioListener.volume = 0f;
        }
    }

    public void showQuitPopUp(bool isShow)
    {
        Managers.UI.popUps.Close(this.gameObject);

        Managers.UI.popUps.ActivateQuitPopUp();
    }
    public void OpenFacebookPage()
    {
        Application.OpenURL(Constants.FACEBOOK_URL);
    }

    public void OpenTwitterPage()
    {
        Application.OpenURL(Constants.TWITTER_URL);
    }

    public void OpenContact()
    {
        Application.OpenURL(Constants.CONTACT_URL);
    }

    public void RateAsset()
    {
        Application.OpenURL(Constants.ASSETSTORE_URL);
    }
}
