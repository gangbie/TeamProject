using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class BaePlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpPower;

    private Vector2 inputDir;
    private SpriteRenderer rend;
    private Rigidbody2D rb;
    private Animator ani;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        transform.position += new Vector3(inputDir.x*moveSpeed*Time.deltaTime,0);
        if (inputDir.x>0)
        {
            rend.flipX = false;
        }
        else if (inputDir.x < 0)
        {
            rend.flipX = true;
        }
        ani.SetFloat("MoveFloat",Mathf.Abs(inputDir.x));
    }

    public void Jump()
    {
        rb.AddForce(Vector2.up*jumpPower, ForceMode2D.Impulse);
    }
    private void OnMove(InputValue value)
    {
        inputDir=value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        Jump();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ani.SetBool("JumpBool", false);
        if(collision.gameObject.tag=="Monster")
            rb.AddForce(Vector2.up*10.0f,ForceMode2D.Impulse);

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ani.SetBool("JumpBool", true); 
    }
}
