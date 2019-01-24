using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;

public enum BlockType
{
    Classical,
    MergeObstacle,
    NotToMergeObstacle,
    Bomb,
}

public class TetrisShape : MonoBehaviour
{
    public Text textMesh;
    public Image spriteRenderer, BmobImage;
    public ParticleSystem combineParticleSystem;
    public ParticleSystem deathParticleSystem;
    public int idColor;
    public bool isRandomStartPostion = false;
    public int[] StartPostionRandomPosX = new int[] { 0, 2, 4, 6, 8 };
    [HideInInspector]
    public ShapeMovementController movementController;
    public bool isMergeObstacle = false;
    public bool isNotToMergeObstacle = false;
    public bool isBmob = false;
    public Sprite NotToMergeObstacleSprite, Bmobsprite;

    private void Awake()
    {
        movementController = GetComponent<ShapeMovementController>();
    }
    public void UpdateGrid() {
        
        if (isRandomStartPostion == true)
        {
            transform.position = new Vector3(StartPostionRandomPosX[UnityEngine.Random.Range(0, StartPostionRandomPosX.Length)], transform.position.y, transform.position.z);
        }

        if (isMergeObstacle|| isNotToMergeObstacle)
        {   
            StartCoroutine(playShowAnim(isMergeObstacle));
            return;
        }

        // Default position not valid? Then it's game over
        if (!Managers.Grid.IsValidGridPosition(this.transform))
        {
            Managers.Game.SetState(typeof(GameOverState));
            Destroy(this.gameObject);
        }
        else
        {
            Managers.Grid.UpdateGrid(this.transform);
        }
    }
    int getRealIdColor(int idColor) {
        return Mathf.FloorToInt(idColor % 13);
    }
    public void UpdateAttribute(int idColor, BlockType type)
    {
        switch (type) {

            case BlockType.Classical:
                UpdateAttribute(idColor);
                break;
            case BlockType.MergeObstacle:
                UpdateAttribute(idColor);
                break;
            case BlockType.NotToMergeObstacle:
                this.idColor = idColor;
                spriteRenderer.sprite = NotToMergeObstacleSprite;
                BmobImage.gameObject.SetActive(false);
				int showText = getShowText(this.idColor);
				textMesh.text = showText.ToString();
                break;
            case BlockType.Bomb:
                this.idColor = idColor;
                spriteRenderer.sprite = Bmobsprite;
                BmobImage.gameObject.SetActive(true);
                textMesh.text = "";
                break;
        }
        
    }

    void UpdateAttribute(int idColor) {
        this.idColor = getRealIdColor(idColor);
//        Color temp = Managers.Palette.TurnRandomColorFromTheme(this.idColor);

        spriteRenderer.sprite = Managers.Palette.GetSprite(this.idColor);
        BmobImage.gameObject.SetActive(false);
        int showText = getShowText(this.idColor);
        textMesh.text = showText.ToString();
    }

    public void UpdateData(int idColor)
    {
        Managers.Score.OnScore(getShowText(idColor));

        this.idColor = getRealIdColor(idColor);

		int showText = getShowText(this.idColor);
		textMesh.text = showText.ToString();

//		Color temp = Managers.Palette.TurnRandomColorFromTheme(this.idColor);

		spriteRenderer.sprite = Managers.Palette.GetSprite(this.idColor);
    }

	int getShowText(int num){
		int showText=2 ;
		for(int i=1; i<=num;i++){
			showText=showText*2;
		}
		return showText;
	}
    public IEnumerator playDeathAnim() {
        deathParticleSystem.gameObject.SetActive(true);
        deathParticleSystem.Play();

        combineParticleSystem.gameObject.SetActive(true);
        combineParticleSystem.Play();

        Managers.Audio.Play(SoundType.electric);

        textMesh.DOFade(0, 1.5f);
        BmobImage.DOFade(0, 1.5f);
        spriteRenderer.DOFade(0, 1.5f).OnComplete(delegate () {
            
        });

        yield return new WaitForSeconds(1.5f);
		Managers.Score.OnScore(getShowText(idColor));
        Destroy(transform.gameObject);
    }

     IEnumerator playShowAnim(bool isMergeObstacle)
    {
        combineParticleSystem.gameObject.SetActive(true);
        combineParticleSystem.Play();

        Managers.Audio.Play(SoundType.lightning);
        Managers.Grid.UpdateGrid(this.transform);

        yield return new WaitForSeconds(1.5f);
        if (isMergeObstacle) {
            Managers.Grid.PlaceShape(this.transform);
        }
    }

}
