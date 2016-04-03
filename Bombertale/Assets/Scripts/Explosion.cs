using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    public float lifespan = .5f;


    void Start()
    {
        Destroy(this.gameObject, lifespan);
       // Invoke("Explode", lifespan);
    }

    //void Explode()
    //{
    //    Destroy(this.gameObject);
    //}
}