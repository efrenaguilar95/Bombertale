using UnityEngine;
using System.Collections;

public class PowerUps : MonoBehaviour {
    public float speed = 1;
    public float bombCD = 0;
    public int exSize = 1;
    
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.gameObject.tag == "PUSpeed")
            {
                other.gameObject.GetComponent<Player>().moveSpeed += 1;
                
            }
            if (this.gameObject.tag == "PUExplosion")
            {
               other.gameObject.GetComponent<Player>().bombSize += 1;
            }
            if (this.gameObject.tag == "PUBomb")
            {
                //decrease bomb cooldown/increase bomb size
                //however we decide to do it
            }
            if (this.gameObject.tag == "PUThrow")
            {
                //give player ability to throw bomb
            }
            if (this.gameObject.tag == "PUKick")
            {
                //give player ability to kick bomb
            }
        }
       Destroy(this.gameObject);
    }
}
