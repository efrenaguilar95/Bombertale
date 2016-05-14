using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    public float lifespan = .5f;
    private int xLoc, yLoc;
    private Mapper map;

    void Awake()
    {
        map = GameObject.Find("Map").GetComponent<Mapper>();
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
        map.grid[xLoc][yLoc] = CellID.Empty;
        Destroy(this.gameObject);
    }
}