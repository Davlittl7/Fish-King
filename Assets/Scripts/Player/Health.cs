using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
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
}
