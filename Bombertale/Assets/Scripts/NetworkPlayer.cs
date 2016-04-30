using UnityEngine;
using System.Collections;

public class NetworkPlayer : MonoBehaviour
{

    public Vector2 worldLocation;
    public Vector2 gridLocation;
    public Direction direction = Direction.NONE;
    public bool isAlive = false;


    //powerup variables
    public float speed = 3f;
    public int bombCount = 1;
    public int explosionRadius = 1;
    public bool isInvulnerable = false;
    public float invulnTimeRemaining;

    //functions
}
