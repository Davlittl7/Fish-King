using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int maxHealth = 3;
    public static int health = 3;
    public static GameObject[] bubbles;

    private void Start()
    {
        bubbles = GameObject.FindGameObjectsWithTag("Health");
    }

    // Update is called once per frame
    void Update()
    {
        //Make sure health is never greater than amount of bubbles
        //So, if player pick up health, won't exceed their bubble amount
        if(health > bubbles.Length) health = bubbles.Length;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HealthPickup")
        {
            //Reverse effect of animation first, then increment health
            if (health < maxHealth)
            {
                bubbles[health].GetComponent<Animator>().SetBool("isHealthGained", true);
                Destroy(collision.gameObject);
                StartCoroutine(regainHealth());
                ++health;
            }

        }
    }

    IEnumerator regainHealth()
    {
        yield return new WaitForSeconds(0.833f);
        bubbles[health - 1].GetComponent<Animator>().SetBool("isHealthGained", false);
    }
}
