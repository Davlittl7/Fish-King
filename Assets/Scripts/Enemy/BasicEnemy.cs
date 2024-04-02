using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    private Transform target;

    public float speed = 3f;

    public float rotateSpeed = 0.5f;

    private Rigidbody2D rb;

    private void Start()
    {
       rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Rotating towards the target
        if (!target)
        {
            GetTarget();
        } else
        {
            RotateTowardsTarget();
        }
    }

    private void FixedUpdate()
    {
        //Moving foward
        rb.velocity = transform.up * speed;
    }

    //Makes the enemy always look at the player
    private void RotateTowardsTarget()
    {
        if (target != null)
        {
            Vector2 targetDir = target.position - transform.position;
            float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
            Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed);
        }
    }

    //Determines the player as the target
    private void GetTarget()
    {
        if(GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
       
    }

    //Destroys Player if they are ran into, destroys enemy if they are shot
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            target = null;
        } else if (collision.gameObject.CompareTag("Bubble"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
