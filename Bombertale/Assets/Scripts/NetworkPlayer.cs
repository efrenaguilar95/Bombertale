using UnityEngine;
using System.Collections;

[System.Serializable]
// I'm sorry, we have to do this because JsonUtility sucks
public struct PlayerData
{
    public Direction direction;
    public bool isAlive;
    public float speed;
    public int bombCount;
    public int explosionRadius;
    public bool isInvulnerable;
    public float invulnTimeRemaining;
    public PlayerData(Direction direction, bool isAlive, float speed, int bombCount, int explosionRadius, bool isInvulnerable, float invulnTimeRemaining)
    {
        this.direction = direction;
        this.isAlive = isAlive;
        this.speed = speed;
        this.bombCount = bombCount;
        this.explosionRadius = explosionRadius;
        this.isInvulnerable = isInvulnerable;
        this.invulnTimeRemaining = invulnTimeRemaining;
    }
}

public class NetworkPlayer : MonoBehaviour
{
    //public Direction direction = Direction.NONE;
    //public bool isAlive = false;


    ////Powerup Variables
    //public float speed = 3f;
    //public int bombCount = 1;
    //public int explosionRadius = 1;
    //public bool isInvulnerable = false;
    //public float invulnTimeRemaining;

    public PlayerData data = new PlayerData(Direction.NONE, false, 3f, 1, 1, false, 0f);


    public Vector2 GetGridLocation()
    {
        return new Vector2((int)this.transform.position.x, (int)this.transform.position.y);
    }
}
