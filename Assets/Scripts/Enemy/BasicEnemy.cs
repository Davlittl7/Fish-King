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
            //Plays animation of losing a bubble and destroys player if health is now 0
            --Health.health;

            //Checks to see if animator is already enabled and uses different ways to run anim if it is
            if (Health.bubbles[Health.health].GetComponent<Animator>().
                isActiveAndEnabled == true)
                Health.bubbles[Health.health].GetComponent<Animator>().SetBool("isHealthLossed", true);
            else
                Health.bubbles[Health.health].GetComponent<Animator>().enabled = true;

            if (Health.health == 0)
            {
                Destroy(collision.gameObject);
                target = null;
            }
            StartCoroutine(playerIsHit());

        } else if (collision.gameObject.CompareTag("Bubble"))
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
