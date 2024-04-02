using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;

    Vector2 movementInput;

    public Rigidbody2D rb;

    public GameObject bubble;

    public Transform bubbleBlow;

    public float bubbleForce = 10f;

    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //Player movement and determining left or right movement animation
        rb.MovePosition(rb.position + movementInput * movementSpeed * Time.fixedDeltaTime);

        //Checks to see if idle animation is needed
        if (movementInput.x == 0) animator.SetBool("isIdle", true);
        else animator.SetBool("isIdle", false);    
    }
    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
        animator.SetFloat("Horizontal", movementInput.x);
    }

    void OnFire()
    {

        GameObject bubbles = Instantiate(bubble, bubbleBlow.position, bubbleBlow.rotation);
        Rigidbody2D rb = bubbles.GetComponent<Rigidbody2D>();
        rb.AddForce(bubbleBlow.up * bubbleForce, ForceMode2D.Impulse);
        
    }

}
