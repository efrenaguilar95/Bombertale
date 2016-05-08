using UnityEngine;
using System.Collections;

public class Singleton<Instance> : MonoBehaviour where Instance : Singleton <Instance> {
    public static Instance instance = null;
    public bool isPersistant;


    public virtual void Awake()
    {
        if(isPersistant)
        {
            if (instance == null)
            {
                instance = this as Instance;
            }

            else
            {
                DestroyObject(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            instance = this as Instance;
        }
    }
}
