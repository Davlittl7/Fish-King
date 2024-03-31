using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;

    Vector2 movementInput;

    private Rigidbody2D rb;

    public GameObject bubble;

    public Transform bubbleBlow;

    //public float bubbleTimer = 0.15f;
    //private float currBubbleTimer;
    //private bool canAttack;
    //Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //currBubbleTimer = bubbleTimer; 
    }

    void FixedUpdate()
    {
       rb.MovePosition(rb.position + movementInput * movementSpeed * Time.fixedDeltaTime);
    }

    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void OnFire()
    {
        //bubbleTimer += Time.deltaTime;
        /*if(bubbleTimer > currBubbleTimer)
        {
            canAttack = true;
        }
        if(canAttack)
        {
            canAttack = false;
            bubbleTimer = 0f;
            Instantiate(bubble, bubbleBlow.position, Quaternion.identity);
        }*/
        Instantiate(bubble, bubbleBlow.position, Quaternion.identity);

    }

}
