using UnityEngine;
using System.Collections;

[System.Serializable]
// I'm sorry, we have to do this because JsonUtility sucks
public struct PlayerData
{
    public string name;
    public Vector2 worldLocation;
    public Direction direction;
    public bool isAlive;
    public float speed;
    public int bombCount;
    public int explosionRadius;
    public bool isInvulnerable;
    public float invulnTimeRemaining;
    public PlayerData(string name, Vector2 worldLocation, Direction direction, bool isAlive, float speed, int bombCount, int explosionRadius, bool isInvulnerable, float invulnTimeRemaining)
    {
        this.name = name;
        this.worldLocation = worldLocation;
        this.direction = direction;
        this.isAlive = isAlive;
        this.speed = speed;
        this.bombCount = bombCount;
        this.explosionRadius = explosionRadius;
        this.isInvulnerable = isInvulnerable;
        this.invulnTimeRemaining = invulnTimeRemaining;
    }

    //Copy constructor
    public PlayerData(PlayerData previousPlayerData)
    {
        this.name = previousPlayerData.name;
        this.worldLocation = previousPlayerData.worldLocation;
        this.direction = previousPlayerData.direction;
        this.isAlive = previousPlayerData.isAlive;
        this.speed = previousPlayerData.speed;
        this.bombCount = previousPlayerData.bombCount;
        this.explosionRadius = previousPlayerData.explosionRadius;
        this.isInvulnerable = previousPlayerData.isInvulnerable;
        this.invulnTimeRemaining = previousPlayerData.invulnTimeRemaining;
    }
}

public class NetworkPlayer : MonoBehaviour
{

    public PlayerData data;
    private GameObject bombPrefab;
    private ClientManager _clientManager;
    private Animator playerAnim;
    private bool isOrigColor = true;
    private Color flashColor;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        this.data = new PlayerData(this.name, this.GetWorldLocation(), Direction.NONE, false, 3f, 1, 1, false, 0f);
        this.bombPrefab = Resources.Load("Bomb") as GameObject;
        _clientManager = GameObject.Find("NetworkManager").GetComponent<ClientManager>();
        playerAnim = this.GetComponent<Animator>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        flashColor = new Color(1.0f, 0, 0);
    }

    void Update()
    {
        this.data.worldLocation = GetWorldLocation();
        if (this.data.invulnTimeRemaining > 0)
        {
            this.data.invulnTimeRemaining -= Time.deltaTime;
            StartCoroutine("Flash");
        }
        else
        {
            this.data.isInvulnerable = false;
            StopCoroutine("Flash");
        }
        if (this.data.direction == Direction.NONE)
        {
            playerAnim.SetBool("Moving", false);
        }
        else
        {
            playerAnim.SetBool("Moving", true);
            switch (this.data.direction)
            {
                case Direction.UP:
                    playerAnim.Play("WalkUp");
                    break;
                case Direction.LEFT:
                    playerAnim.Play("WalkLeft");
                    break;
                case Direction.DOWN:
                    playerAnim.Play("WalkDown");
                    break;
                case Direction.RIGHT:
                    playerAnim.Play("WalkRight");
                    break;
                default:
                    break;
            }
        }
        if(!isOrigColor && !this.data.isInvulnerable)
        {
            spriteRenderer.color = Color.white;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Explosion"))
        {
            //Server I is ded?
            _clientManager.SendTriggerRequest(TriggerType.Explosion);
        }
        if (other.CompareTag("PUSpeed"))
        {
            //Server I can go fast?
            _clientManager.SendTriggerRequest(TriggerType.SpeedUp);
        }
        if (other.CompareTag("PUExplosion"))
        {
            //Server I can has bigger bombs?
            _clientManager.SendTriggerRequest(TriggerType.ExplosionRadius);
        }
        if (other.CompareTag("PUBomb"))
        {
            //Server I can has more bombs?
            _clientManager.SendTriggerRequest(TriggerType.BombCount);
        }
        if (other.CompareTag("PUDetermine"))
        {
            //Server I is determined?
            _clientManager.SendTriggerRequest(TriggerType.Determination);
        }        
    }

    private void toggleFlash()
    {
        if (!isOrigColor)
            spriteRenderer.color = Color.white;
        else
            spriteRenderer.color = flashColor;
        isOrigColor = !isOrigColor;
    }

    private IEnumerator Flash()
    {
        while(this.data.isInvulnerable)
        {
            toggleFlash();
            yield return new WaitForSeconds(.15f);
        }
    }

    //public void Translate(Vector2 deltaPos)
    //{
    //    this.transform.Translate(deltaPos);
    //    this.data.worldLocation = this.GetWorldLocation();
    //}

    //public void SetPosition(Vector2 absoluteLocation)
    //{
    //    this.transform.position = absoluteLocation;
    //    this.data.worldLocation = this.GetWorldLocation();
    //}

    public Vector2 GetWorldLocation()
    {
        return this.transform.position;
    }

    public Vector2 GetGridLocation()
    {
        return new Vector2((int)this.transform.position.x, (int)this.transform.position.y);
    }

    public void DropBomb()
    {
        GameObject newBomb = (GameObject)Instantiate(bombPrefab, this.GetGridLocation(), Quaternion.identity);
        Bomb bombScript = newBomb.GetComponent<Bomb>();
        bombScript.size = this.data.explosionRadius;
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), newBomb.GetComponent<Collider2D>());
    }


}
