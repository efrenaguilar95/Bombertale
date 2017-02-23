using UnityEngine;
using System.Collections;

public class LocalGameBomb : MonoBehaviour
{
    public float lifespan = 2.5f;
    public int size = 1;
    public GameObject exCenter;
    public GameObject exLeft;
    public GameObject exRight;
    public GameObject exUp;
    public GameObject exDown;
    public GameObject exHori;
    public GameObject exVert;

    LocalMapper map;
    LocalGameManager localGameManager;

    AudioSource bombSound;

    void Awake()
    {
        bombSound = GameObject.Find("GameAudioManager").GetComponent<GameAudio>().bombSound;
        localGameManager = GameObject.Find("LocalGameManager").GetComponent<LocalGameManager>();
    }

    void Start()
    {
        Invoke("Explode", lifespan);
    }

    void Explode()
    {
        Destroy(this.gameObject);
        bombSound.Play();
        Instantiate(exCenter, this.transform.position, Quaternion.identity);

        ExplosionDirection(Vector3.up);
        ExplosionDirection(Vector3.down);
        ExplosionDirection(Vector3.right);
        ExplosionDirection(Vector3.left);
    }

    void ExplosionDirection(Vector3 direction)
    {
        GameObject connectPrefab = null, endPrefab = null;
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
            
            if (localGameManager.charMap[xLoc][yLoc] == CellID.SoftBlock || localGameManager.charMap[xLoc][yLoc] == CellID.HardBlock || localGameManager.charMap[xLoc][yLoc] == CellID.ConeBlock)
            {
                if (localGameManager.charMap[xLoc][yLoc] == CellID.SoftBlock)
                {
                    //Debug.Log(string.Format("Block broken at {0}, {1}", xLoc, yLoc));
                    SoftBlock localSoftBlock = localGameManager.gameObjectMap[xLoc][yLoc].GetComponent<SoftBlock>();
                    if (localSoftBlock != null)
                    {
                        localSoftBlock.GetRekt(xLoc, yLoc);
                    }
                    else
                    {
                        //map.gameObjectGrid[xLoc][yLoc].GetComponent<NetworkSoftBlock>().GetRekt();
                        //localGameManager.gameObjectMap[xLoc][yLoc].GetComponent<NetworkSoftBlock>().GetRekt();
                    }
                }
                return;
            }

            if (i < size)
                Instantiate(connectPrefab, new Vector2(xLoc, yLoc), Quaternion.identity);
            else
                Instantiate(endPrefab, new Vector2(xLoc, yLoc), Quaternion.identity);
            localGameManager.charMap[xLoc][yLoc] = CellID.Explosion;
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
