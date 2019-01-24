using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeShow : MonoBehaviour {

    public Text textMesh;
    public Image spriteRenderer, BmobImage;
    public int idColor;
    public Sprite Bmobsprite;
    // Use this for initialization
    void Start () {
         
	}
    private void OnEnable()
    {
    }
    // Update is called once per frame
    void Update () {
		
	}
    int getRealIdColor(int idColor)
    {
        return Mathf.FloorToInt(idColor % 13);
    }
    int getShowText(int num)
    {
        int showText = 2;
        for (int i = 1; i <= num; i++)
        {
            showText = showText * 2;
        }
        return showText;
    }
    public void UpdateAttribute(int idColor,bool Bomb)
    {
        BmobImage.gameObject.SetActive(false);
        if (Bomb)
        {
            this.idColor = idColor;
            spriteRenderer.sprite = Bmobsprite;
            BmobImage.gameObject.SetActive(true);
            textMesh.text = "";
        }
        else
        {
            this.idColor = getRealIdColor(idColor);
            spriteRenderer.sprite = Managers.Palette.GetSprite(this.idColor);

            int showText = getShowText(this.idColor);
            textMesh.text = showText.ToString();

        }
       
    }
}
