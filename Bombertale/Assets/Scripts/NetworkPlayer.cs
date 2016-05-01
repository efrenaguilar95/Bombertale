using UnityEngine;
using System.Collections;

public class NetworkPlayer : MonoBehaviour
{
    ////Copy constrcutor
    //public NetworkPlayer(NetworkPlayer previousNetworkPlayer)
    //{
    //    direction = previousNetworkPlayer.direction;
    //    isAlive = previousNetworkPlayer.isAlive;
    //    speed = previousNetworkPlayer.speed;
    //    bombCount = previousNetworkPlayer.bombCount;
    //    explosionRadius = previousNetworkPlayer.explosionRadius;
    //    isInvulnerable = previousNetworkPlayer.isInvulnerable;
    //    invulnTimeRemaining = previousNetworkPlayer.invulnTimeRemaining;
    //}

    public Direction direction = Direction.NONE;
    public bool isAlive = false;


    //Powerup Variables
    public float speed = 3f;
    public int bombCount = 1;
    public int explosionRadius = 1;
    public bool isInvulnerable = false;
    public float invulnTimeRemaining;


    public Vector2 GetGridLocation()
    {
        return new Vector2((int)this.transform.position.x, (int)this.transform.position.y);
    }
}
