using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
    public float lifespan = 2.5f;
    public int size = 1;
    public GameObject exCenter;
    public GameObject exLeft;
    public GameObject exRight;
    public GameObject exUp;
    public GameObject exDown;
    public GameObject exHori;
    public GameObject exVert;

    Mapper map;

    void Awake()
    {
        map = GameObject.Find("Map").GetComponent<Mapper>();
    }

    void Start () {
        Invoke("Explode", lifespan);
	}

    void Explode()
    {
        Destroy(this.gameObject);
        Instantiate(exCenter, this.transform.position, Quaternion.identity);

        ExplosionDirection(Vector3.up);
        ExplosionDirection(Vector3.down);
        ExplosionDirection(Vector3.right);
        ExplosionDirection(Vector3.left);
    }

    void ExplosionDirection(Vector3 direction)
    {
        GameObject connectPrefab=null, endPrefab=null;
        if (direction == Vector3.up)
        {
            connectPrefab = exVert;
            endPrefab = exUp;
        }
        else if (direction == Vector3.down)
        {
            connectPrefab = exVert;
            endPrefab = exDown;
        }
        else if (direction == Vector3.left)
        {
            connectPrefab = exHori;
            endPrefab = exLeft;
        }
        else if (direction == Vector3.right)
        {
            connectPrefab = exHori;
            endPrefab = exRight;
        }


        int bombX = (int)this.transform.position.x;
        int bombY = (int)this.transform.position.y;
        for (int i = 1; i <= size; i++)
        {
            int xLoc = bombX + (i * (int)direction.x);
            int yLoc = bombY + (i * (int)direction.y);

            if (map.grid[xLoc][yLoc] != ".")    //Might break if we add IDs for players
            {
                if (map.grid[xLoc][yLoc] == "S")
                {                    
                    map.gameObjectGrid[xLoc][yLoc].GetComponent<SoftBlock>().Fizzle();
                }
                return;
            }

            if (i < size)
                Instantiate(connectPrefab, new Vector2(xLoc, yLoc), Quaternion.identity);
            else
                Instantiate(endPrefab, new Vector2(xLoc, yLoc), Quaternion.identity);
        }        
    }

    void OnTriggerEnter2D(Collider2D hitObject)
    {
        if (hitObject.CompareTag("Explosion"))
        {
            Explode();
        }
    }

    void OnTriggerExit2D(Collider2D hitObject)
    {
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), hitObject.GetComponent<Collider2D>(), false);
    }
}
