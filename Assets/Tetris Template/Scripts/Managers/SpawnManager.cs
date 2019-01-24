using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

	public GameObject shapeTypes;
    public void Spawn()
	{
        Managers.Game.stats.sncyGridData();
        TetrisShape tetrisShape;
        int rand;
        bool nextShapeIsBmob;
        if (Managers.Game.stats.nextShapeID >= 0)
        {
            rand = Managers.Game.stats.nextShapeID;
            nextShapeIsBmob = Managers.Game.stats.nextShapeIsBmob;
        }
        else
        {
            rand = UnityEngine.Random.Range(Managers.Config.startColorID, Managers.Config.endColorID);
            nextShapeIsBmob = false;
            if (Managers.Config.isShowNotToMergeObstacle)
            {
                if (Random.Range(0, 15)==5)
                {
                    rand = 0;
                    nextShapeIsBmob = true;
                }
            }
        }
        //下一个方块预算
        Managers.Game.stats.nextShapeID = UnityEngine.Random.Range(Managers.Config.startColorID, Managers.Config.endColorID);
        Managers.Game.stats.nextShapeIsBmob = false;

        if (Managers.Config.isShowNotToMergeObstacle)
        {
            if (Random.Range(0, 15)==5)
            {
                Managers.Game.stats.nextShapeID = 0;
                Managers.Game.stats.nextShapeIsBmob = true;
            }
        }

        ShapeShow nextShape = Managers.UI.inGameUI.nextShape;
        if (Managers.Config.isShowNextShape == true)
        {
            nextShape.gameObject.SetActive(true);
            nextShape.UpdateAttribute(Managers.Game.stats.nextShapeID,Managers.Game.stats.nextShapeIsBmob);
        }
        else
        {
            nextShape.gameObject.SetActive(false);
        }
        //实例化
        tetrisShape = InstantiateBlock(false,false, nextShapeIsBmob);

        if (nextShapeIsBmob)
        {
            tetrisShape.UpdateAttribute(rand,BlockType.Bomb);
        }
        else {
            tetrisShape.UpdateAttribute(rand, BlockType.Classical);
        }

        tetrisShape.UpdateGrid();


        Managers.Game.currentShape = tetrisShape;
        Managers.Input.isActive = true;
        Managers.Game.stats.currentShapeIdColor = tetrisShape.idColor ;
    }

    public void InstantiateMergeObstacle(int[] pos) {
        TetrisShape tetrisShape = InstantiateBlock(true,false,false);
        int rand = Managers.Game.stats.ObstacleIndex;
        Managers.Game.stats.ObstacleIndex++;
        if (Managers.Game.stats.ObstacleIndex >= 13) {
            Managers.Game.stats.ObstacleIndex = 0;
        }
        tetrisShape.UpdateAttribute(rand, BlockType.MergeObstacle);

        tetrisShape.transform.position = new Vector3(pos[0], pos[1], tetrisShape.transform.position.z);
        tetrisShape.UpdateGrid();
    }

    public void InstantiateNotToMergeObstacle(int[] pos)
    {
        TetrisShape tetrisShape = InstantiateBlock(false,true, false);
        tetrisShape.UpdateAttribute(Managers.Config.NotToMergeObstacleScore, BlockType.NotToMergeObstacle);
        tetrisShape.transform.position = new Vector3(pos[0], pos[1], tetrisShape.transform.position.z);
        tetrisShape.UpdateGrid();
    }

    public TetrisShape InstantiateBlock(bool isMergeObstacle ,bool isNotToMergeObstacle, bool isBmob)
    {
        GameObject temp =Instantiate(shapeTypes) ;
        temp.transform.parent = Managers.Game.blockHolder;
        TetrisShape tetrisShape = temp.GetComponent<TetrisShape>();
        tetrisShape.isMergeObstacle = isMergeObstacle;
        tetrisShape.isNotToMergeObstacle = isNotToMergeObstacle;
        tetrisShape.isBmob = isBmob;
        return tetrisShape;
    }

}
