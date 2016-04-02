using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject player1, player2, player3, player4;
    Player p1Script;
    Player p2Script;
    Player p3Script;
    Player p4Script;

    void Awake () {
        p1Script = player1.GetComponent<Player>();
        p2Script = player2.GetComponent<Player>();
        p3Script = player3.GetComponent<Player>();
        p4Script = player4.GetComponent<Player>();
    }
		
	void Update () {
        //Player1 Movement
        if (Input.GetKeyDown(KeyCode.W))
        {
            p1Script.verticalMovement = Direction.UP;
            if (!p1Script.isMoving())
                p1Script.toggleMovement();
            p1Script.SetAnim("WalkUp");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            p1Script.verticalMovement = Direction.DOWN;
            if (!p1Script.isMoving())
                p1Script.toggleMovement();
            p1Script.SetAnim("WalkDown");
        }
        else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)){
            p1Script.verticalMovement = Direction.NONE;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            p1Script.horizontalMovement = Direction.LEFT;
            if(!p1Script.isMoving())
                p1Script.toggleMovement();
            p1Script.SetAnim("WalkLeft");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            p1Script.horizontalMovement = Direction.RIGHT;
            if(!p1Script.isMoving())
                p1Script.toggleMovement();
            p1Script.SetAnim("WalkRight");
        }
        else if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            p1Script.horizontalMovement = Direction.NONE;
        }
        if (p1Script.horizontalMovement == Direction.NONE && p1Script.verticalMovement == Direction.NONE)
        {
            //p1Script.SetAnim("IdleDown");
            if(p1Script.isMoving())
                p1Script.toggleMovement();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            p1Script.DropBomb();    
        }

        //Player2 Movement
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            p2Script.verticalMovement = Direction.UP;
            if (!p2Script.isMoving())
                p2Script.toggleMovement();
            p2Script.SetAnim("WalkUp");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            p2Script.verticalMovement = Direction.DOWN;
            if (!p2Script.isMoving())
                p2Script.toggleMovement();
            p2Script.SetAnim("WalkDown");
        }
        else if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            p2Script.verticalMovement = Direction.NONE;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            p2Script.horizontalMovement = Direction.LEFT;
            if (!p2Script.isMoving())
                p2Script.toggleMovement();
            p2Script.SetAnim("WalkLeft");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            p2Script.horizontalMovement = Direction.RIGHT;
            if (!p2Script.isMoving())
                p2Script.toggleMovement();
            p2Script.SetAnim("WalkRight");
        }
        else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            p2Script.horizontalMovement = Direction.NONE;
        }

        if (p2Script.horizontalMovement == Direction.NONE && p2Script.verticalMovement == Direction.NONE)
        {
            if (p2Script.isMoving())
                p2Script.toggleMovement();
        }
        //Player2 bomb
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            p2Script.DropBomb();
        }

        //Player3 Movement
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            p3Script.verticalMovement = Direction.UP;
            if (!p3Script.isMoving())
                p3Script.toggleMovement();
            p3Script.SetAnim("WalkUp");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            p3Script.verticalMovement = Direction.DOWN;
            if (!p3Script.isMoving())
                p3Script.toggleMovement();
            p3Script.SetAnim("WalkDown");
        }
        else if (!Input.GetKey(KeyCode.Keypad5) && !Input.GetKey(KeyCode.Keypad2))
        {
            p3Script.verticalMovement = Direction.NONE;
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            p3Script.horizontalMovement = Direction.LEFT;
            if (!p3Script.isMoving())
                p3Script.toggleMovement();
            p3Script.SetAnim("WalkLeft");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            p3Script.horizontalMovement = Direction.RIGHT;
            if (!p3Script.isMoving())
                p3Script.toggleMovement();
            p3Script.SetAnim("WalkRight");
        }
        else if (!Input.GetKey(KeyCode.Keypad1) && !Input.GetKey(KeyCode.Keypad3))
        {
            p3Script.horizontalMovement = Direction.NONE;
        }

        if (p3Script.horizontalMovement == Direction.NONE && p3Script.verticalMovement == Direction.NONE)
        {
            if (p3Script.isMoving())
                p3Script.toggleMovement();
        }
        //Player3 bomb
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            p3Script.DropBomb();
        }
        //Player4 Movement
        if (Input.GetKeyDown(KeyCode.I))
        {
            p4Script.verticalMovement = Direction.UP;
            if (!p4Script.isMoving())
                p4Script.toggleMovement();
            p4Script.SetAnim("WalkUp");
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            p4Script.verticalMovement = Direction.DOWN;
            if (!p4Script.isMoving())
                p4Script.toggleMovement();
            p4Script.SetAnim("WalkDown");
        }
        else if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K))
        {
            p4Script.verticalMovement = Direction.NONE;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            p4Script.horizontalMovement = Direction.LEFT;
            if (!p4Script.isMoving())
                p4Script.toggleMovement();
            p4Script.SetAnim("WalkLeft");
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            p4Script.horizontalMovement = Direction.RIGHT;
            if (!p4Script.isMoving())
                p4Script.toggleMovement();
            p4Script.SetAnim("WalkRight");
        }
        else if (!Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.L))
        {
            p4Script.horizontalMovement = Direction.NONE;
        }

        if (p4Script.horizontalMovement == Direction.NONE && p4Script.verticalMovement == Direction.NONE)
        {
            if (p4Script.isMoving())
                p4Script.toggleMovement();
        }
        //Player4 bomb
        if (Input.GetKeyDown(KeyCode.P))
        {
            p4Script.DropBomb();
        }
    }
}
