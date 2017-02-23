using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Broken for local game
public class SoftBlock : MonoBehaviour {

    LocalMapper map;

    public GameObject[] powerUps;

    private bool isQuitting = false;

    private int x, y;

    public void GetRekt(int xLoc, int yLoc)
    {
        x = xLoc;
        y = yLoc;
        Fizzle();
    }

    public void Fizzle()
    {
        //Play Animations
        this.GetComponent<Animator>().Play("SoftBlockFizzle");
        float animTime = .5f;
        Destroy(this.gameObject, animTime);
    }

    //private void ClearMap()
    //{
    //    map.grid[xLoc][yLoc] = ".";
    //    Destroy(map.gameObjectGrid[xLoc][yLoc]);
    //}
    void OnApplicationQuit()
    {
        isQuitting = true;  
    }
    void OnDestroy()
    {
        if (!isQuitting)
        {
            //map.grid[xLoc][yLoc] = CellID.Empty;
            List<List<char>> mapgrid = LocalMapper.StringToMap(LocalMapper.mapString);
            float rand = Random.Range(0, 100);
            int randPU = Random.Range(0, powerUps.Length);
            //GameObject power = powerUps[randPU];
            if (rand >= 30)
            {
                mapgrid[x][y] = randPU.ToString()[0];
                //Instantiate(power, this.transform.position, Quaternion.identity);
            }
            else
            {
                mapgrid[x][y] = CellID.Empty;
            }
            string newMapstring = LocalMapper.MapToString(mapgrid);
            LocalMapper.mapString = newMapstring;
        }
    }
}
