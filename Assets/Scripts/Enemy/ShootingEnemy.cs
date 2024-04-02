using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    private Transform target;

    public float rotateSpeed = 0.5f;

    public float speed = 5f;

    private Rigidbody2D rb;

    public float deactivateT = 3f;

    public GameObject bubble;
    private GameObject player;

    public Transform bubbleBlow;

    public float bubbleTimer;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeactivateGameObject", deactivateT);
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Rotating towards the target
        if (!target)
        {
            GetTarget();
        }
        else
        {
            RotateTowardsTarget();
        }
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance < 10)
            {
                bubbleTimer += Time.deltaTime;

                if (bubbleTimer > 2)
                {
                    bubbleTimer = 0;
                    Move();
                }
            }
        }
    }

    void Move()
    {
        Instantiate(bubble, bubbleBlow.position, Quaternion.identity);
    }

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

    private void GetTarget()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

    }

    //Deactivates the oyster to save memory
    void DeactivateGameObject()
    {
        Destroy(GameObject.FindGameObjectWithTag("Oyster"));
    }

    //Destroys Player if they are ran into, destroys enemy if they are shot
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            target = null;
        }
        else if (collision.gameObject.CompareTag("Bubble"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
