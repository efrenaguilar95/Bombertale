﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public GameObject bomb;

    [HideInInspector]
    public Direction horizontalMovement, verticalMovement;
    public float moveSpeed = 3f;
    public int bombSize = 1;
    public float speedPU = 1f;
    public int sizePU = 1;

    private Vector2 startPos;
    Animator playerAnim;

    void Start()
    {
        playerAnim = this.GetComponent<Animator>();
        startPos = this.transform.position;
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
            Die();
        }
        else if(!hitObject.CompareTag("Bomb")){
            if (hitObject.CompareTag("PUSpeed"))
            {
                moveSpeed += speedPU;
            }
            if (hitObject.CompareTag("PUExplosion"))
            {
                bombSize += sizePU;
            }
            Destroy(hitObject.gameObject);
        }
    }

    public void DropBomb()
    {
        GameObject newBomb = (GameObject)Instantiate(bomb, MyLocation(), Quaternion.identity);
        newBomb.GetComponent<Bomb>().size = bombSize;
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), newBomb.GetComponent<Collider2D>());
    }

    private Vector2 MyLocation()
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
        Debug.Log(this.gameObject.name + " Died");
    }
}
