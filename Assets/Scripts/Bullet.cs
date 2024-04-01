using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float deactivateT = 3f;

    // Start is called before the first frame update
    //Invokes deactivation function
    void Start()
    {
        Invoke("DeactivateGameObject", deactivateT);
    }

    // Update is called once per frame
    //Calls for the bubble to move across the screen
    void Update()
    {
        Move();
    }

    //Moves the bubble along the y-axis
    void Move()
    {
        Vector2 temp = transform.position;
        temp.y += speed * Time.deltaTime;
        transform.position = temp;
    }
    
    //Deactivates the bubble to save memory
    void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
