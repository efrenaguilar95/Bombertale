using UnityEngine;
using System.Collections;

//Broken for local game
public class SoftBlock : MonoBehaviour {


    public GameObject[] powerUps;

    private bool isQuitting = false;

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
            //map.grid[xLoc][yLoc] = CellID.Empty;
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
