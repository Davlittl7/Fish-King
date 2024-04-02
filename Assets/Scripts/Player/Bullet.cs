using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float deactivateT = 3f;
    //public GameObject player;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    //Invokes deactivation function
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("DeactivateGameObject", deactivateT);
    }
    
    //Deactivates the bubble to save memory
    void DeactivateGameObject()
    {
        Destroy(gameObject);
    }

}
