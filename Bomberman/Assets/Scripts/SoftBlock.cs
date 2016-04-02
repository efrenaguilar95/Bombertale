using UnityEngine;
using System.Collections;

public class SoftBlock : MonoBehaviour {

    Mapper map;
    int xLoc, yLoc;

    void Awake()
    {
        map = GameObject.Find("Map").GetComponent<Mapper>();
        xLoc = (int)this.transform.position.x;
        yLoc = (int)this.transform.position.y;
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

    void OnDestroy()
    {
        map.grid[xLoc][yLoc] = ".";
    }
}
