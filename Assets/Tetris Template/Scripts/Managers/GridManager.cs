using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class Column
{
    public Transform[] row = new Transform[20];
}

public class GridManager : MonoBehaviour
{
    public Column[] gameGridcol = new Column[11];

    public List<Transform> allFindRows = new List<Transform>();
    public int currentFindNum = 0;
    public List<Transform> needFindObjs = new List<Transform>();

    public List<Transform> downAnimObjs = new List<Transform>();



    private float downAnimTime = 0.1f;
    private float moveAnimTime = 0.1f;
    private float BombAnimTime = 1.5f;
    private float soundTime = 0.5f;

    public int currentColorId;

    public int currentLianXiaoCount = 0;


    public void PlaceShape(Transform obj)
    {
        if (Managers.Game.State.ToString() == "GamePlayState")
        {
            if (currentFindNum > 0)
            {
                needFindObjs.Add(obj);
            }
            else
            {
                currentLianXiaoCount = 0;
                StartCoroutine(DeleteRows(obj));
            }
            //currentLianXiaoCount = 0;
            //StartCoroutine(DeleteRows(obj));
        }
    }

    IEnumerator DeleteRows(Transform obj)
    {
        currentFindNum = currentFindNum + 1;
        if (obj)
        {
            if (haveSamwAnimObj(needFindObjs, obj))
            {
                for (int i = 0; i < needFindObjs.Count; i++)
                {
                    if (needFindObjs[i] == obj)
                    {
                        needFindObjs.RemoveAt(i);
                        break;
                    }
                }
            }

            allFindRows.Clear();
            downAnimObjs.Clear();

            if (obj.GetComponent<TetrisShape>().isBmob)
            {
                List<int[]> deleteLineArray = removeAroundBlock(obj);

				//出现爆破的时候强行设置到excellent级别 currentLianXiaoCount>=7就是 
				currentLianXiaoCount = currentLianXiaoCount + 7;
				//currentLianXiaoCount++;

                //等待爆炸消除时间
                if (deleteLineArray.Count > 0)
                {
                    yield return new WaitForSeconds(BombAnimTime);
                    downBlock(deleteLineArray);
                    //等待消除满足2048的一排方格下落时间
                    yield return new WaitForSeconds(downAnimTime);
                }

                //继续判断是否消除
                if (needFindObjs.Count > 0)
                {
                    StartCoroutine(DeleteRows(obj));
                }
                else
                {
                    needFindObjs.Clear();
                }
            }
            else
            {
                FindOne((int)obj.position.x, (int)obj.position.y);
                if (allFindRows.Count >= 2)
                {
                    allFindRows.Remove(obj);
                    List<int[]> Clone_AllFindRows_Pos = removeSameBlock(obj);
                    yield return new WaitForSeconds(moveAnimTime);
        
                    if (!haveSamwAnimObj(needFindObjs, obj))
                    {
                        needFindObjs.Add(obj);
                    }

                    downBlock(Clone_AllFindRows_Pos);
                    //等待下落时间
                    yield return new WaitForSeconds(downAnimTime);

                     

                    if (currentColorId >= Managers.Config.deleteID)
                    {	
						//出现爆破的时候强行设置到excellent级别 currentLianXiaoCount>=7就是 
						currentLianXiaoCount = currentLianXiaoCount + 7;
						//currentLianXiaoCount++;

						if (Managers.Config.isDestroyAround == true) {
                            //等待音效时间
                            yield return new WaitForSeconds(soundTime);

                            //List<int[]> deleteLineArray = removeLineBlock(obj);
                            List<int[]> deleteLineArray =  removeAroundBlock(obj);
                            //等待爆炸消除时间
                            if (deleteLineArray.Count > 0) {
                                yield return new WaitForSeconds(BombAnimTime);
                                downBlock(deleteLineArray);
                                //等待消除满足2048的一排方格下落时间
                                yield return new WaitForSeconds(downAnimTime);
                            }
                        }
                    }
                    //继续判断是否消除
                    if (needFindObjs.Count > 0)
                    {
                        StartCoroutine(DeleteRows(obj));
                    }
                    else {
                        needFindObjs.Clear();
                    }
                }
                else
                {
                    Recursion_DeleteRows();
                }
            }
        }
        else
        {

            Recursion_DeleteRows();
        }
        currentFindNum = currentFindNum - 1;
        //New shape will be spawned
        if (currentFindNum <= 0)
        {
            Managers.Game.isShowLianXiaoAnim(currentLianXiaoCount);
            yield return new WaitForSeconds(0.5f);
            if (Managers.Game.currentShape == null)
            {
                Managers.Spawner.Spawn();
            }
        }
        yield break;
    }

   public int[] getCurrentTheHighest() {
        int[] pos=new int[2];
        for (int y = 3; y <= Managers.Config.theHighest; y=y+2)
        {
            for (int x = 0; x <= Managers.Config.theMostWide; x=x+2)
            {
                if (gameGridcol[x].row[y] == null)
                {
                    pos[0] = x;
                    pos[1] = y;
                    return pos;
                }
            }
        }
        return pos;
    }
    void Recursion_DeleteRows()
    {
        if (needFindObjs.Count > 0)
        {
            for (int i = 0; i < needFindObjs.Count; i++)
            {
                if (needFindObjs[i] == null)
                {
                    needFindObjs.RemoveAt(i);
                }
            }
            for (int i = 0; i < needFindObjs.Count; i++)
            {
                if (needFindObjs[i] != null)
                {
                    StartCoroutine(DeleteRows(needFindObjs[i]));
                    break;
                }
            }
        }
    }
    List<int[]> removeLineBlock(Transform obj)
    {
        List<int[]> deleteLineArray = new List<int[]>();
        for (int x = 0; x < 11; x++)
        {
            if (gameGridcol[x].row[(int)obj.position.y] != null)
            {
                int[] pos = new int[2];
                pos[0] = x;
                pos[1] = (int)obj.position.y;
                deleteLineArray.Add(pos);

                destroyShape(x, (int)obj.position.y);
            }
        }
        return deleteLineArray;
    }

    List<int[]> removeAroundBlock(Transform obj)
    {   

        List<int[]> deletAroundArray = new List<int[]>();
        int posX = (int)obj.position.x;
        int posY = (int)obj.position.y;
        
        for (int i = 0; i <= 3; i++)
        {
            int finalX = 0;
            int finalY= 0;
            bool canAdd = false;
            if (i == 0)
            {
                int x = 0;
                x = posX - 2;
                if (isOverStep(x, true)==false)
                {
                    if (gameGridcol[x].row[posY]!=null)
                    {
                        finalX = x;
                        finalY = posY;
                        canAdd = true;
                    }
                }
            }
            else if (i == 1)
            {
                int x = 0;
                x = posX + 2;
                if (isOverStep(x, true) == false)
                {
                    if (gameGridcol[x].row[posY]!=null)
                    {
                        finalX = x;
                        finalY = posY;
                        canAdd = true;
                    }
                }
            }
            else if (i == 2)
            {
                int y = 0;
                y = posY - 2;

                if (isOverStep(y, false) == false)
                {
                    if ( gameGridcol[posX].row[y]!=null)
                    {
                        finalX = posX;
                        finalY = y;
                        canAdd = true;
                    }
                }
            }
            else if (i == 3)
            {
                int y = 0;
                y = posY + 2;
                if (isOverStep(y, false) == false)
                {
                    if (gameGridcol[posX].row[y]!=null)
                    {
                        finalX = posX;
                        finalY = y;
                        canAdd = true;
                    }
                }
            }
            if (canAdd == true)
            {
                int[] pos = new int[2];
                pos[0] = finalX;
                pos[1] = finalY;
                deletAroundArray.Add(pos);

                destroyShape(finalX, finalY);
            }
        }

       
        int[] myPos = new int[2];
        myPos[0] = posX;
        myPos[1] = posY;
        deletAroundArray.Add(myPos);

        destroyShape(posX, posY);

        Managers.Game.stats.updateMoney(16);
     
        return deletAroundArray;
    }

    private void destroyShape(int x,int y)
    {
         
        TetrisShape tetrisShape = gameGridcol[x].row[y].GetComponent<TetrisShape>();
        StartCoroutine(tetrisShape.playDeathAnim());

        gameGridcol[x].row[y] = null;

        for (int i = 0; i < needFindObjs.Count; i++)
        {
            if (needFindObjs[i] == gameGridcol[x].row[y])
            {
                needFindObjs.RemoveAt(i);
            }
        }
    }

    List<int[]> removeSameBlock(Transform obj)
    {
        List<int[]> Clone_AllFindRows_Pos = new List<int[]>();
        foreach (Transform target in allFindRows)
        {
            if (target != null)
            {
//                TetrisShape tetrisShape = target.GetComponent<TetrisShape>();
                int posX = (int)target.position.x;
                int posY = (int)target.position.y;

                int[] pos = new int[2];
                pos[0] = posX;
                pos[1] = posY;
                Clone_AllFindRows_Pos.Add(pos);

                for (int i = 0; i < needFindObjs.Count; i++)
                {
                    if (needFindObjs[i] == target)
                    {
                        needFindObjs.RemoveAt(i);
                    }
                }

                gameGridcol[posX].row[posY] = null;
                currentLianXiaoCount++;
                Tweener moveToTargetAnim = target.DOMove(obj.position, moveAnimTime).SetEase(Ease.Linear);
                moveToTargetAnim.OnComplete(delegate ()
                {
                    Destroy(target.gameObject);
                });
            }
        }  

        int finalUpdateCpunt;
        finalUpdateCpunt = allFindRows.Count;
      
        int idColor = obj.GetComponent<TetrisShape>().idColor;
        currentColorId = idColor + allFindRows.Count;
        obj.GetComponent<TetrisShape>().UpdateData(currentColorId);

        //obj.GetComponent<TetrisShape>().playCombinAnim();
        Managers.Audio.Play(SoundType.LineClear);

        return Clone_AllFindRows_Pos;
    }

    void downBlock(List<int[]> downObjInfo)
    {

        downObjInfo.Sort((x, y) => x[0].CompareTo(y[0]));//升序
        downObjInfo.Sort((x, y) => -x[1].CompareTo(y[1]));//降序

        foreach (int[] pos in downObjInfo)
        {
            int x = pos[0];
            int y = pos[1];
            for (int i = y + 2; i <20; ++i)
            {
                if (isOverStep(i-2, false) == false) {
					if (gameGridcol[x].row[i] != null && gameGridcol[x].row[i - 2] == null
						&&gameGridcol[x].row[i]!=Managers.Game.currentShape)
                    {
                        gameGridcol[x].row[i - 2] = gameGridcol[x].row[i];
                        gameGridcol[x].row[i] = null;

                        Transform downAnimObj = gameGridcol[x].row[i - 2];
                        downAnimObjs.Add(downAnimObj);

                        if (!haveSamwAnimObj(needFindObjs, downAnimObj))
                        {
                            needFindObjs.Add(downAnimObj);
                        }
                    }
                }
            }
        }

        downAnimObjs.Sort((obj1, obj2) => obj1.position.x.CompareTo(obj2.position.x));//升序
        downAnimObjs.Sort((obj1, obj2) => -obj1.position.y.CompareTo(obj2.position.y));//降序

        List<Transform> ingAnimList = new List<Transform>();
        for (int i = 0; i < downAnimObjs.Count; i++)
        {
            // downAnimObj[i].position += new Vector3(0, -2, 0);
            Transform downAnimObj = downAnimObjs[i];
            if (!haveSamwAnimObj(ingAnimList, downAnimObj))
            {
                ingAnimList.Add(downAnimObj);
                downAnimObj.DOMoveY(downAnimObj.position.y - getSameAnimObjCount(downAnimObj) * 2, downAnimTime).SetEase(Ease.Linear);
            }
        }
        ingAnimList.Clear();
        downAnimObjs.Clear();
    }

    int getSameAnimObjCount(Transform downObj)
    {
        int count = 0;
        foreach (Transform obj in downAnimObjs)
        {
            if (downObj == obj)
            {
                count++;
            }
        }
        return count;
    }

    public bool isOverStep(int index, bool horizontal)
    {
        if (horizontal)
        {
            if (index < 0 || index > Managers.Config.theMostWide)
            {
                return true;
            }

        }
        else
        {
            int highest = Managers.Config.theHighest;
            if (index < 2 || index > highest)
            {
                return true;
            }
        }

        return false;
    }

    public void FindOne(int posX, int posY)
    {
        for (int i = 0; i <= 3; i++)
        {

            if (i == 0)
            {
                int x = 0;
                x = posX - 2;
                if (isOverStep(x, true))
                {
                    // print("越界Left");
                }
                else
                {
                    if (IsSame(gameGridcol[posX].row[posY], gameGridcol[x].row[posY]))
                    {
                        if (!haveSamwAnimObj(allFindRows, gameGridcol[x].row[posY]))
                        {
                            // print("加进来一个Left");
                            allFindRows.Add(gameGridcol[x].row[posY]);
                            FindOne(x, posY);
                        }
                    }
                    else
                    {
                        // print("不一样或者没有Left");
                    }
                }

            }
            else if (i == 1)
            {
                int x = 0;
                x = posX + 2;
                if (isOverStep(x, true))
                {
                    // print("越界Right");
                }
                else
                {
                    if (IsSame(gameGridcol[posX].row[posY], gameGridcol[x].row[posY]))
                    {
                        if (!haveSamwAnimObj(allFindRows, gameGridcol[x].row[posY]))
                        {
                            // print("加进来一个Right");
                            allFindRows.Add(gameGridcol[x].row[posY]);
                            FindOne(x, posY);
                        }
                    }
                    else
                    {
                        // print("不一样或者没有Right");
                    }
                }

            }
            else if (i == 2)
            {
                int y = 0;
                y = posY - 2;
                if (isOverStep(y, false))
                {
                    // print("越界Down");
                }
                else
                {
                    if (IsSame(gameGridcol[posX].row[posY], gameGridcol[posX].row[y]))
                    {
                        if (!haveSamwAnimObj(allFindRows, gameGridcol[posX].row[y]))
                        {
                            // print("加进来一个Down");
                            allFindRows.Add(gameGridcol[posX].row[y]);
                            FindOne(posX, y);
                        }
                    }
                    else
                    {
                        // print("不一样或者没有Down");
                    }
                }

            }
            else if (i == 3)
            {
                int y = 0;
                y = posY + 2;
                if (isOverStep(y, false))
                {
                    //print("越界Top");
                }
                else
                {
                    if (IsSame(gameGridcol[posX].row[posY], gameGridcol[posX].row[y]))
                    {
                        if (!haveSamwAnimObj(allFindRows, gameGridcol[posX].row[y]))
                        {
                            // print("加进来一个Top");
                            allFindRows.Add(gameGridcol[posX].row[y]);
                            FindOne(posX, y);
                        }
                    }
                    else
                    {
                        // print("不一样或者没有Top");
                    }
                }

            }
        }

    }

    public bool IsSame(Transform currentObj, Transform otherObj)
    {
        if (currentObj && otherObj)
        {
            TetrisShape selfTetrisShape = currentObj.GetComponent<TetrisShape>();
            TetrisShape otherTetrisShape = otherObj.GetComponent<TetrisShape>();
            if (selfTetrisShape.idColor == otherTetrisShape.idColor && selfTetrisShape.isNotToMergeObstacle == false)
            {
                return true;
            }
        }
        return false;
    }

    public bool haveSamwAnimObj(List<Transform> target, Transform obj)
    {
        foreach (Transform item in target)
        {
            if (item == obj)
            {
                return true;
            }
        }
        return false;
    }

    public bool InsideBorder(Vector3 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x <= Managers.Config.theMostWide&& (int)pos.y >= 2);
    }

    public bool IsValidGridPosition(Transform obj)
    {

        if (!InsideBorder(obj.position))
        {
            return false;
        }
        // Block in grid cell (and not part of same group)?
        if (gameGridcol[(int)obj.position.x].row[(int)obj.position.y] != null &&
            gameGridcol[(int)obj.position.x].row[(int)obj.position.y] != obj)
        {
            return false;
        }

        return true;
    }

    public void UpdateGrid(Transform obj)
    {
        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 11; x++)
            {
                if (gameGridcol[x].row[y] != null)
                {
                    if (gameGridcol[x].row[y] == obj)
                        gameGridcol[x].row[y] = null;
                }
            }
        }

        gameGridcol[(int)obj.position.x].row[(int)obj.position.y] = obj;
    }

    public void ClearBoard(bool isRevive)
    {
		ClearGrid ();
        Managers.Score.ResetScore(isRevive);
    }

	public void ClearGrid(){

		for (int y = 0; y < 20; y++)
		{
			for (int x = 0; x < 11; x++)
			{
				if (gameGridcol[x].row[y] != null)
				{
					Destroy(gameGridcol[x].row[y].gameObject);
					gameGridcol[x].row[y] = null;
				}
			}
		}

		foreach (Transform t in Managers.Game.blockHolder)
			Destroy(t.gameObject);
	}

}