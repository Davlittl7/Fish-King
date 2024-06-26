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

    public GameObject bubble;
    private GameObject player;

    public Transform bubbleBlow;

    public float bubbleTimer;

    // Start is called before the first frame update
    void Start()
    {
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

    //Destroys Player if they are ran into, destroys enemy if they are shot
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Plays animation of losing a bubble and destroys player if health is now 0
            --Health.health;

            //Checks to see if animator is already enabled and uses different ways to run anim if it is
            if (Health.bubbles[Health.health].GetComponent<Animator>().
                isActiveAndEnabled == true)
                Health.bubbles[Health.health].GetComponent<Animator>().SetBool("isHealthLossed", true);
            else
                Health.bubbles[Health.health].GetComponent<Animator>().enabled = true;
            
            if (Health.health == 0) { 
                Destroy(collision.gameObject);
                target = null;
            }
            StartCoroutine(playerIsHit());

        }
        else if (collision.gameObject.CompareTag("Bubble"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }

    IEnumerator playerIsHit()
    {
        //Make sure player's movement isn't affected by enemy collison
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.2f);
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

        //Show health lost animation
        yield return new WaitForSeconds(0.833f);
        Health.bubbles[Health.health].GetComponent<Animator>().SetBool("isHealthLossed", false);
        Health.bubbles[Health.health].GetComponent<Animator>().enabled = false;

    }
}
