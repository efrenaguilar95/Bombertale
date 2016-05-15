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
    AudioSource bombSound;

    //This will break local game
    private ClientManager _clientManager;
    private ServerGameManager _serverGameManager;

    void Awake()
    {
        map = GameObject.Find("Map").GetComponent<Mapper>();
        bombSound = GameObject.Find("GameAudioManager").GetComponent<GameAudio>().bombSound;
        _clientManager = GameObject.Find("NetworkManager").GetComponent<ClientManager>();
        _serverGameManager = GameObject.Find("GameManager").GetComponent<ServerGameManager>();
    }

    void Start () {
        Invoke("Explode", lifespan);
	}

    void Explode()
    {
        Destroy(this.gameObject);
        bombSound.Play();
        Instantiate(exCenter, this.transform.position, Quaternion.identity);
        //map.grid[(int)this.transform.position.x][(int)this.transform.position.y] = CellID.Explosion;
        _clientManager.charMap[(int)this.transform.position.x][(int)this.transform.position.y] = CellID.Explosion;
        if (_serverGameManager != null)
            _serverGameManager.charMap[(int)this.transform.position.x][(int)this.transform.position.y] = CellID.Explosion;

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

            //if (_serverGameManager.charMap[xLoc][yLoc] == CellID.SpeedUp || _serverGameManager.charMap[xLoc][yLoc] == CellID.BombUp ||
            //    _serverGameManager.charMap[xLoc][yLoc] == CellID.ExplosionUp || _serverGameManager.charMap[xLoc][yLoc] == CellID.Determination)
            //{

            //}

            //if (map.grid[xLoc][yLoc] == CellID.SoftBlock || map.grid[xLoc][yLoc] == CellID.HardBlock || map.grid[xLoc][yLoc] == CellID.ConeBlock)    //Might break if we add IDs for players
            if (_clientManager.charMap[xLoc][yLoc] == CellID.SoftBlock || _clientManager.charMap[xLoc][yLoc] == CellID.HardBlock || _clientManager.charMap[xLoc][yLoc] == CellID.ConeBlock)
            {
                //if (map.grid[xLoc][yLoc] == CellID.SoftBlock)
                if (_clientManager.charMap[xLoc][yLoc] == CellID.SoftBlock)
                {
                    //SoftBlock localSoftBlock = map.gameObjectGrid[xLoc][yLoc].GetComponent<SoftBlock>();
                    SoftBlock localSoftBlock = _clientManager.gameObjectMap[xLoc][yLoc].GetComponent<SoftBlock>();
                    if (localSoftBlock != null)
                    {
                        localSoftBlock.GetRekt();
                    }
                    else
                    {
                        //map.gameObjectGrid[xLoc][yLoc].GetComponent<NetworkSoftBlock>().GetRekt();
                        _clientManager.gameObjectMap[xLoc][yLoc].GetComponent<NetworkSoftBlock>().GetRekt();
                    }
                }
                return;
            }

            if (i < size)
                Instantiate(connectPrefab, new Vector2(xLoc, yLoc), Quaternion.identity);
            else
                Instantiate(endPrefab, new Vector2(xLoc, yLoc), Quaternion.identity);
            //map.grid[xLoc][yLoc] = CellID.Explosion;
            _clientManager.charMap[xLoc][yLoc] = CellID.Explosion;
            if (_serverGameManager != null)
                _serverGameManager.charMap[xLoc][yLoc] = CellID.Explosion;
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
