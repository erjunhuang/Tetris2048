using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatorManger : MonoBehaviour {
    public Image image;
    public Animator animator;
	// Use this for initialization
	void Start () {
    }
    public void show(int count) {
        string imageRes= "lianxiao3";
        if (count >= 3 && count < 5)
        {
            imageRes = "lianxiao3";
			Managers.Audio.Play(SoundType.good);
        }
        else if (count >= 5 && count < 7)
        {
            imageRes = "lianxiao5";
			Managers.Audio.Play(SoundType.great);
        }
        else if( count >= 7) {
            imageRes = "lianxiao7";
			Managers.Audio.Play(SoundType.excellent);
        }
        image.sprite = Resources.Load(imageRes, typeof(Sprite)) as Sprite;
        image.SetNativeSize();
        animator.SetBool("isPlay", true);
        Destroy(this.gameObject, 2f);
    }
}
