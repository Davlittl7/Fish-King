using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float deactivateT = 3f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeactivateGameObject", deactivateT);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector2 temp = transform.position;
        temp.y += speed * Time.deltaTime;
        transform.position = temp;
    }
    void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
