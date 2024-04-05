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
        Vector3 direction = player.transform.position - transform.position;
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
        if(collision.gameObject.CompareTag("Player"))
        {
            //Plays animation of losing a bubble and destroys player if health is now 0
            --Health.health;
            Health.bubbles[Health.health].GetComponent<Animator>().enabled = true;
            if (Health.health == 0) Destroy(collision.gameObject);
            Destroy(gameObject);
            StartCoroutine(playerIsHit());
        } else if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Bubble"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator playerIsHit()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.2f);
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    }
}
