using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;

    Vector2 movementInput;

    private Rigidbody2D rb;

    public GameObject bubble;

    public Transform bubbleBlow;
    private Transform playerHead;

    [SerializeField]
    public float rotateSpeed;

    public float bubbleForce = 10f;

    Animator animator;

    void Start()
    {
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
        //Mapping x and y inputs to two float variables
        float horizontal = movementInput.x;
        float vertical = movementInput.y;

        //Connects and normalizes movements
        Vector2 movementDirection = new Vector2(horizontal, vertical);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
        movementDirection.Normalize();

        //Causes the movement
        transform.Translate(movementDirection * movementSpeed * inputMagnitude * Time.deltaTime, Space.World);

        //Rotation
        if(movementDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
        }

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
