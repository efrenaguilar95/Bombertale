using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalGameExplosion : MonoBehaviour
{
    public float lifespan = .5f;
    private int xLoc, yLoc;

    void Awake()
    {
        xLoc = (int)this.transform.position.x;
        yLoc = (int)this.transform.position.y;
    }

    void Start()
    {
        //Destroy(this.gameObject, lifespan);
        StartCoroutine(this.Destroy(lifespan));
    }

    private IEnumerator Destroy(float time)
    {
        yield return new WaitForSeconds(time);
        List<List<char>> mapgrid = LocalMapper.StringToMap(LocalMapper.mapString);
        mapgrid[xLoc][yLoc] = CellID.Empty;
        string newMapstring = LocalMapper.MapToString(mapgrid);
        LocalMapper.mapString = newMapstring;
        Destroy(this.gameObject);
    }
}