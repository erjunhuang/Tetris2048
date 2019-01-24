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

public enum SoundType
{
    electric,
    lightning,
    Lose,
    Win,
    UIClick,
    Drop,
    LineClear,
	good,
	great,
	excellent,
}

public class AudioManager : MonoBehaviour {

    public AudioSource musicSource;
	public AudioSource soundSource;
    public AudioSource effectSource;

    public AudioClip gameMusic;
    public AudioClip dropSound;
    public AudioClip lineClearSound;
	public AudioClip uiClick;
    public AudioClip loseSound;
	public AudioClip goodSound;
	public AudioClip greatSound;
	public AudioClip excellentSound;

    #region Effeck Spesific
    public AudioClip electricSound;
    public AudioClip lightningSound;
    #endregion
    private void Start()
    {
         
    }
    public void Play(SoundType type) {
        switch (type) {
            case SoundType.Lose:
                soundSource.clip = loseSound;
                soundSource.Play();
                break;
            case SoundType.UIClick:
                soundSource.clip = uiClick;
                soundSource.Play();
                break;
            case SoundType.Drop:
                soundSource.clip = dropSound;
                soundSource.Play();
                break;
            case SoundType.LineClear:
                soundSource.clip = lineClearSound;
                soundSource.Play();
                break;
			case SoundType.good:
				soundSource.clip = goodSound;
				soundSource.Play();
				break;
			case SoundType.great:
				soundSource.clip = greatSound;
				soundSource.Play();
				break;
			case SoundType.excellent:
				soundSource.clip = excellentSound;
				soundSource.Play();
				break;
            case SoundType.electric:

                AudioSource electricEffect = new GameObject("sound").AddComponent<AudioSource>();
                electricEffect.clip = electricSound;
                electricEffect.Play();
                Destroy(electricEffect.gameObject, 3f);
                break;
            case SoundType.lightning:
                AudioSource lightningEffect = new GameObject("sound").AddComponent<AudioSource>();
                lightningEffect.clip = lightningSound;
                lightningEffect.Play();
                Destroy(lightningEffect.gameObject, 3f);
                break;
            default:
                break;
        }
    }

    public void PlayGameMusic()
    {
        musicSource.clip = gameMusic;
        musicSource.Play();
    }

    public void StopGameMusic()
    {
        musicSource.Stop();
    }

    public void SetSoundMusicVolume(float value)
    {
        float temp = value + musicSource.volume;
        if (temp < 0 || temp > 1)
            return;
        else
            musicSource.volume += value;
    }

    public void SetSoundFxVolume(float value)
	{
		float temp = value + soundSource.volume;
		if (temp < 0 || temp > 1)
			return;
		else
			soundSource.volume += value;
	}
}
