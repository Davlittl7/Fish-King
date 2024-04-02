using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;

    Vector2 movementInput;
    Vector2 mousePos;
    Vector2 smoothMove;
    Vector2 smVelocity;

    private Rigidbody2D rb;

    public GameObject bubble;
    public GameObject player;

    public Transform bubbleBlow;
    private Transform playerHead;

    public float rotateSpeed = 0.5f;

    public float bubbleForce = 10f;

    Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHead = GameObject.FindGameObjectWithTag("Head").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        //Player movement and determining left or right movement animation
        rb.MovePosition(rb.position + movementInput * movementSpeed * Time.fixedDeltaTime);
        float angle = Mathf.Atan2(movementInput.y, movementInput.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        rb.transform.rotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed);

        //Checks to see if idle animation is needed
        if (movementInput == Vector2.zero) animator.SetBool("isIdle", true);
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
