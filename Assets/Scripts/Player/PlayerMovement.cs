using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    private enum State
    {
        Normal,
        Dodging,
        Hit
    }

    Vector2 movementInput;
    Vector2 dodgeDir;
    Vector2 hitDir;

    private Rigidbody2D rb;

    public GameObject bubble;

    public Transform bubbleBlow;

    private State state;

    [SerializeField]
    public float rotateSpeed;

    private float dodgeSpeed;
    private int dodgeAmount = 1;
    private bool isVulnerable = true;
    Collider2D fishBody;

    public float bubbleForce = 10f;
    public float knockbackForce = 100f;


    Animator animator;


    private void Awake()
    {
        state = State.Normal;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        fishBody = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        //Being in a state of normal means regular movement, dodging gives player a slight boost in speed
        if (state == State.Normal)
        {
            Movement();
        }
        else if (state == State.Dodging)
        {
            Dodge();
        }
    }

    void Movement()
    {
        dodgeSpeed = 0f;
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
        if (movementDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
        }

        //Checks to see if idle animation is needed
        if (movementInput == Vector2.zero) animator.SetBool("isIdle", true);
        else animator.SetBool("isIdle", false);
    }

    void Dodge()
    {
        float horizontal = movementInput.x;
        float vertical = movementInput.y;


        Vector2 movementDirection = new Vector2(horizontal, vertical);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
        movementDirection.Normalize();
        transform.Translate(movementDirection * dodgeSpeed * inputMagnitude * Time.deltaTime, Space.World);

        //Slowly brings down the speed at which the player is going
        float dodgeSpeedDrop = 5f;
        dodgeSpeed -= dodgeSpeed * dodgeSpeedDrop * Time.deltaTime;

        //Once it hits its minimum speed it sets the state back to normal and starts a delay on the next dodge
        float dodgeSpeedMin = 5f;
        if (dodgeSpeed < dodgeSpeedMin)
        {
            state = State.Normal;
        }
        StartCoroutine(DodgeDelay());
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

    void OnDodge()
    {
        if (dodgeAmount > 0)
        {
            dodgeSpeed = 30f;
            state = State.Dodging;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Bubble" && isVulnerable)
        {
            //state = State.Hit;
            Collider2D collider = collision.collider;

            //Calculates direction player will be taken from knockback
            Vector2 direction = (collider.transform.position - transform.position).normalized;
            Vector2 knockBackDir = new Vector2(direction.y, direction.x);
            float knockMagnitude = Mathf.Clamp01(knockBackDir.magnitude);
            knockBackDir.Normalize();

            //Calculates force of the knockback
            Vector2 knockback = direction * knockbackForce;

            rb.AddForce(knockBackDir * knockbackForce, ForceMode2D.Impulse);

            //Start invulnerability period
            StartCoroutine(Invulnerable());
        }
    }


    public IEnumerator DodgeDelay()
    {
        dodgeAmount = 0;
        yield return new WaitForSeconds(1f);
        dodgeAmount = 1;
    }

    public IEnumerator Invulnerable()
    {
        isVulnerable = false;
        fishBody.enabled = !fishBody.enabled;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        fishBody.enabled = !fishBody.enabled;
        isVulnerable = true;
    }
}