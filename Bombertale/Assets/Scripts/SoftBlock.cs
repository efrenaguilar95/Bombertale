using UnityEngine;
using System.Collections;

public class SoftBlock : MonoBehaviour {


    public GameObject[] powerUps;

    private bool isQuitting = false;

    Mapper map;
    int xLoc, yLoc;
    void Awake()
    {
        map = GameObject.Find("Map").GetComponent<Mapper>();
        xLoc = (int)this.transform.position.x;
        yLoc = (int)this.transform.position.y;  
    }

    public void GetRekt()
    {
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
            map.grid[xLoc][yLoc] = ".";
            float rand = Random.Range(0, 100);
            int randPU = Random.Range(0, powerUps.Length);
            GameObject power = powerUps[randPU];
            if (rand >= 30)
            {
                Instantiate(power, this.transform.position, Quaternion.identity);
            }
        }
    }
}
