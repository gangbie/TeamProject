using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float movePower;
    [SerializeField] private float jumpPower;
    [SerializeField] LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer renderer;
    private Vector2 inputDir;
    private bool isGround;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Move();
    }

    //private void FixedUpdate()
    //{
    //    GroundCheck();
    //}

    public void Move()
    {
        if (inputDir.x < 0 && rb.velocity.x > -maxSpeed)
            rb.AddForce(Vector2.right * inputDir.x * movePower, ForceMode2D.Force);
        else if (inputDir.x > 0 && rb.velocity.x < maxSpeed)
            rb.AddForce(Vector2.right * inputDir.x * movePower, ForceMode2D.Force);
    }

    public void Jump()
    {
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
    private void OnMove(InputValue value)
    {
        Debug.Log("Move");
        inputDir = value.Get<Vector2>();
        anim.SetFloat("moveSpeed", Mathf.Abs(inputDir.x));
        if (inputDir.x > 0)
            renderer.flipX = false;
        else if (inputDir.x < 0)
            renderer.flipX = true;
    }
    private void OnJump(InputValue value)
    {
        //if (isGround)
            Jump();
    }

    //private void GroundCheck()
    //{
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundLayer);
    //
    //    if (hit.collider != null)
    //    {
    //        isGround = true;
    //        anim.SetBool("IsGround", true);
    //        Debug.DrawRay(transform.position, new Vector3(hit.point.x, hit.point.y, 0) - transform.position, Color.red);
    //    }
    //    else
    //    {
    //        isGround = false;
    //        anim.SetBool("IsGround", false);
    //        Debug.DrawRay(transform.position, Vector3.down * 1.5f, Color.green);
    //    }
    //}

}