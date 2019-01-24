using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Launch : MonoBehaviour {
    public RawImage rawImage;
    // Use this for initialization
    void Start () {

        rawImage.DOFade(1, 1).OnComplete(delegate () {
            SceneManager.LoadScene("Tetris Template/Scenes/GamePlay5x8");
        });
    }
	
	// Update is called once per frame
	void Update () {
	}
}
