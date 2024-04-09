using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pearl : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;

    public float deactivateT = 3f;
    public float force;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        Invoke("DeactivateGameObject", deactivateT);

        //Sets the direction of where the player is and fires a shot
        Vector2 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
    }

    //Deactivates Pearl
    void DeactivateGameObject()
    {
        Destroy(gameObject);
    }

    //If player is hit by the pearl they are destoryed
    //Gets rid of oyster if they collide with a wall
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

            if (Health.health == 0) Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Bubble"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator playerIsHit()
    {
        //Show health lost animation 
        yield return new WaitForSeconds(0.833f);
        Health.bubbles[Health.health].GetComponent<Animator>().SetBool("isHealthLossed", false);
        Health.bubbles[Health.health].GetComponent<Animator>().enabled = false;
    }
}