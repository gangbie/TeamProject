using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class BaePlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpPower;
    private bool isGround = true;

    public UnityEvent OnJumped;
    public UnityEvent OnDied;
    public UnityEvent OnScored;
    //public UnityEvent OnDamage;

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
        if (isGround)
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

            isGround = false;
        }
    }
    private void OnMove(InputValue value)
    {
        inputDir=value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        Jump();
    }

    //피격시 무적
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Ground")
            isGround = true;
        if (collision.gameObject.tag=="Monster")
        {
            OnDamaged(collision.transform.position);
        }
    }

    public void OnDamaged(Vector2 targetPos)
    {
        gameObject.layer = 10;
        

        rend.color = new Color(1, 1, 1 ,0.4f);

        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rb.velocity = new Vector2(dirc * 3, 3);

        Invoke("OffDamaged", 2);
    }
    public void OffDamaged()
    {
        gameObject.layer = 8;
        rend.color = new Color(1, 1, 1, 1);
    }

    //점프시 모션 변경및 몬스 터 밟기
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Gound")
            isGround = true;
        ani.SetBool("JumpBool", false);
        if(collision.gameObject.tag=="Monster")
        {
            rb.velocity = new Vector2(rb.velocity.x, 17);
        }
            

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
        ani.SetBool("JumpBool", true); 
    }
}
