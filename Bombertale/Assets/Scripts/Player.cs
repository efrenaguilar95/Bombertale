using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public GameObject bomb;    

    [HideInInspector]
    public Direction horizontalMovement, verticalMovement;
    [HideInInspector]
    public bool isAlive = true;
    public float moveSpeed = 3f;
    public float maxSpeed = 6f;
    public int bombSize = 1;
    public float speedPU = .5f;
    public int sizePU = 1;
    public bool isDetermined = false;
    public int bombCount = 1;
    public float determinedTime = 5f;
    Mapper map;

    //private Vector2 startPos;
    Animator playerAnim;

    void Start()
    {
        playerAnim = this.GetComponent<Animator>();
        //startPos = this.transform.position;
    }

    void FixedUpdate()
    {
        switch (this.verticalMovement)
        {
            case Direction.UP:
                this.transform.Translate(new Vector2(0, moveSpeed * Time.deltaTime));
                break;
            case Direction.DOWN:
                this.transform.Translate(new Vector2(0, -moveSpeed * Time.deltaTime));
                break;
            default:
                break;
        }

        switch (this.horizontalMovement)
        {
            case Direction.RIGHT:
                this.transform.Translate(new Vector2(moveSpeed * Time.deltaTime, 0));
                break;
            case Direction.LEFT:
                this.transform.Translate(new Vector2(-moveSpeed * Time.deltaTime, 0));
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D hitObject)
    {
        if (hitObject.CompareTag("Explosion"))
        {
            if (new Vector2(hitObject.transform.position.x, hitObject.transform.position.y) == GridLocation())
            {
                Die();
            } 
        }
        else if (hitObject.CompareTag("PUSpeed"))
        {
            if (moveSpeed < maxSpeed)
            {
                moveSpeed += speedPU;
            }
            Destroy(hitObject.gameObject);
        }
        else if (hitObject.CompareTag("PUExplosion"))
        {
            bombSize += sizePU;
            Destroy(hitObject.gameObject);
        } 
        else if (hitObject.CompareTag("PUBomb"))
        {
           bombCount++;
           Destroy(hitObject.gameObject);
        }
        else if (hitObject.CompareTag("PUDetermine"))
        {
            isDetermined = true;
            Invoke("Undetermined", determinedTime);
            Destroy(hitObject.gameObject);
        }
    }

    public void DropBomb()
    {
        if (bombCount > 0)
        {
            bombCount--;
            GameObject newBomb = (GameObject)Instantiate(bomb, GridLocation(), Quaternion.identity);
            Bomb bombScript = newBomb.GetComponent<Bomb>();
            bombScript.size = bombSize;
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), newBomb.GetComponent<Collider2D>());
            Invoke("RefillBombCount", bombScript.lifespan);
        }        
    }

    private void RefillBombCount()
    {
        bombCount++;
    }
    private void Undetermined()
    {
        isDetermined = false;
    }

    public Vector2 GridLocation()
    {
        return new Vector2(Mathf.FloorToInt(this.transform.position.x), Mathf.FloorToInt(this.transform.position.y));
    }

    public void SetAnim(string animation)
    {
        playerAnim.Play(animation);
    }

    public void toggleMovement()
    {
        playerAnim.SetBool("Moving", !playerAnim.GetBool("Moving"));
    }

    public bool isMoving()
    {
        return playerAnim.GetBool("Moving");
    }


    private void Die()
    {
        if (isDetermined == false)
        {
            Debug.Log(this.gameObject.name + " Died");
            isAlive = false;
            //Spawn Napstablook
            GameObject napstablook = (GameObject)Instantiate(Resources.Load("Napstablook"), this.transform.position, Quaternion.identity);
            Destroy(napstablook, 1.5f);
        }
    }

}