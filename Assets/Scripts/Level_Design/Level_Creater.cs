using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Creater : MonoBehaviour
{
    public Vector2Int size = new Vector2Int(2, 2),
                      prevSize = Vector2Int.zero;
    public List<GameObject> bricksList = new List<GameObject>();
    public GameObject brick;
    public Vector2 brickMargin = new Vector2(2, 2),
                   prevMargin = Vector2.zero;
    Coroutine updateBricks;
    bool visible = true;

    public bool initilaized = false;

    public void UpdateBricksList()
    {
        try { StopCoroutine(updateBricks); } catch { }
        updateBricks = StartCoroutine(UpdateThemBricksList());
    }

    public void UpdateGameObject()
    {
        List<GameObject> newList = new List<GameObject>();

        // Delete all the objects
        foreach(GameObject gobj in bricksList)
        {
            newList.Add(Instantiate(brick, gobj.transform.position, gobj.transform.rotation, this.transform));
            newList[newList.Count - 1].SetActive(gobj.activeSelf);
            Brick b = newList[newList.Count - 1].GetComponent<Brick>();
            b.health = gobj.GetComponent<Brick>().health;
            b.id = gobj.GetComponent<Brick>().id;
            DestroyImmediate(gobj);
        }

        bricksList = new List<GameObject>(newList);
    }

    public IEnumerator UpdateThemBricksList()
    {
        int index = 0;
        List<GameObject> toRemove = new List<GameObject>();

        if (bricksList.Count != 0)
        {
            foreach (GameObject ind in bricksList)
            {
                try
                {
                    Brick br = ind.GetComponent<Brick>();
                    if (br.id.x > size.x - 1 || br.id.y > size.y - 1)
                        ind.SetActive(false);
                }
                catch (MissingReferenceException)
                {
                    toRemove.Add(ind);
                }
            }

            for (int i = toRemove.Count; i-- > 0;)
            {
                bricksList.Remove(toRemove[i]);
            }
        }

        GameObject gobj;
        for (int iRow = 0; iRow < size.y; iRow++)
        {
            for (int iCol = 0; iCol < size.x; iCol++)
            {
                index = iRow * size.x + iCol;
                Brick bricky;

                try
                {
                    try
                    {
                        bricky = bricksList[index].GetComponent<Brick>();

                        bricksList[index].transform.position = new Vector3(iCol * brick.transform.localScale.x + brick.transform.localScale.x,
                                                                                iRow * brick.transform.localScale.y + brick.transform.localScale.y, 0);

                        if (!bricky.Toggled)
                            bricky.gameObject.SetActive(true);
                    }
                    catch (MissingReferenceException)
                    {
                        bricksList.RemoveAt(index);

                        InstatiateBrick(iRow, iCol);
                    }
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    InstatiateBrick(iRow, iCol);
                }


                gobj = bricksList[index];
                GameObject prevGobj;
                if (iCol != 0)
                {
                    prevGobj = bricksList[index - 1];
                    bricksList[index].transform.position = new Vector3(prevGobj.transform.position.x + brickMargin.x + brick.transform.localScale.x,
                                                               gobj.transform.position.y,
                                                               gobj.transform.position.z);
                }

                if (iRow != 0)
                {
                    prevGobj = bricksList[index - size.x];
                    bricksList[index].transform.position = new Vector3(gobj.transform.position.x,
                                                           prevGobj.transform.position.y + brickMargin.y + brick.transform.localScale.y,
                                                           gobj.transform.position.z);
                }

                bricky = gobj.GetComponent<Brick>();
                bricky.id = new Vector2Int(iCol, iRow);
            }
        }

        yield return null;
    }

    void InstatiateBrick(int iRow, int iCol)
    {
        bricksList.Add(Instantiate(brick, new Vector3(iCol * brick.transform.localScale.x + brick.transform.localScale.x, iRow * brick.transform.localScale.y + brick.transform.localScale.y, 0), brick.transform.rotation, transform));

        GameObject gobj = bricksList[(int)(bricksList.Count - 1)];
        gobj.hideFlags = HideFlags.HideInHierarchy;
        Brick bricky = gobj.GetComponent<Brick>();

        if (bricky == null)
            bricky = gobj.AddComponent<Brick>();
    }

    public void ToggleVisible(bool status)
    {
        for (int iRow = 0; iRow < size.y; iRow++)
        {
            for (int iCol = 0; iCol < size.x; iCol++)
            {
                bricksList[iRow * size.x + iCol].SetActive(status);
            }
        }
    }
}
