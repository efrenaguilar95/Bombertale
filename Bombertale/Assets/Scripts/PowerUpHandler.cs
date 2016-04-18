using UnityEngine;
using System.Collections;

public class PowerUpHandler : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Explosion"))
        {
            Destroy(this.gameObject);
        }
    }
}
