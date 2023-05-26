using BossState;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class Boss : MonoBehaviour
{
    Rigidbody2D rb;
    public Transform player;
    public float movePower;
    public float jumpPower;
    public float attackRange;
    public float chargePower;
    private float lastJumpTime = 0;
    public float JumpCoolTime = 3;
    private StateBase[] states;
    private State curState;
    public Vector2 zero = new Vector2(0, 0);
    public Vector2 dir;
    public Animator anim;
    public UnityEvent OnAttacked;
    public UnityEvent OnAttackedEnd;
    public UnityEvent OnBossDead;
    public float waitTime;
    public int bossHP = 3;
    public bool attacked = false;
    float attackedtime = 0;
    public bool viewRight = false;
    public bool viewLeft = true;
    public float lastChargeTime = 0;
    public float ChargeCoolTime = 5;
    public UnityEvent OnCharged;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        states = new StateBase[(int)State.Size];
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Trace] = new TraceState(this);
        states[(int)State.Attacked] = new AttackedState(this);
        states[(int)State.Page2] = new Page2State(this);
        states[(int)State.Death] = new DeathState(this);
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        states[(int)curState].Enter();
        StartCoroutine(CoroutineWait());
        curState = State.Idle;
    }

    private void Update()
    {
        states[(int)curState].Update();
        if (attackedtime > 1)
        {
            attacked = false;
            attackedtime = 0;
        }
        attackedtime += Time.deltaTime;
    }
    public void StateChange(State state)
    {
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Enter();
    }

    public void Jump()
    {
        dir = (player.position - transform.position);
        if (lastJumpTime > JumpCoolTime)
        {
            anim.SetTrigger("Jump");
            if (dir.x > zero.x)
            {
                rb.AddForce(Vector2.right * movePower, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(Vector2.left * movePower, ForceMode2D.Impulse);
            }
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            lastJumpTime = 0;
        }
        lastJumpTime += Time.deltaTime;
        if (dir.x > zero.x)
        {
            if (viewLeft)
            {
                transform.Rotate(Vector3.up, 180);
                viewRight = true;
                viewLeft = false;
            }
        }
        else
        {
            if (viewRight)
            {
                transform.Rotate(Vector3.up, 180);
                viewRight = false;
                viewLeft = true;
            }
        }
    }
    public void Charge()
    {
        dir = (player.position - transform.position);
        if (lastChargeTime > ChargeCoolTime)
        {
            if (dir.x > zero.x)
            {
                Debug.Log("Rcharge");
                rb.AddForce(Vector2.right * chargePower, ForceMode2D.Impulse);
            }
            else
            {
                Debug.Log("Lcharge");
                rb.AddForce(Vector2.left * chargePower, ForceMode2D.Impulse);
            }
            lastChargeTime = 0;
        }
        lastChargeTime += Time.deltaTime;
        if (dir.x > zero.x)
        {
            if (viewLeft)
            {
                transform.Rotate(Vector3.up, 180);
                viewRight = true;
                viewLeft = false;
            }
        }
        else
        {
            if (viewRight)
            {
                transform.Rotate(Vector3.up, 180);
                viewRight = false;
                viewLeft = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        attacked = true;
    }
    public void Death()
    {
        OnBossDead?.Invoke();
        Destroy(gameObject, 4f);
    }
    IEnumerator CoroutineWait()
    {
        yield return new WaitForSeconds(13);
    }
}

namespace BossState
{
    public enum State {Idle, Trace, Attacked, Page2, Death, Size}

    public class IdleState : StateBase
    {
        private Boss boss;
        float idleTime = 0;

        public IdleState(Boss boss)
        {
            this.boss = boss;
        }
        public override void Enter()
        {
            Debug.Log("IdleEnter");
        }
        public override void Exit()
        {
            Debug.Log("IdleExit");
        }

        public override void Update()
        {
            if(boss.bossHP == 3)
            {
                if (idleTime > 3)
                {
                    boss.StateChange(State.Trace);
                    idleTime = 0;
                }
                idleTime += Time.deltaTime;
            }
            else if(boss.bossHP == 2)
            {
                if (idleTime > 3)
                {
                    boss.JumpCoolTime = 2;
                    boss.StateChange(State.Trace);
                    idleTime = 0;
                }
                idleTime += Time.deltaTime;
            }
            else if (boss.bossHP == 1)
            {
                if (idleTime > 3)
                {
                    boss.StateChange(State.Page2);
                    idleTime = 0;
                }
                idleTime += Time.deltaTime;
            }
        }
    }

    public class TraceState : StateBase
    {
        private Boss boss;

        public TraceState(Boss boss)
        {
            this.boss = boss;
        }
        public override void Enter()
        {
            Debug.Log("TraceEnter");
        }
        public override void Exit()
        {
            Debug.Log("TraceExit");
        }
        public override void Update()
        {
            boss.Jump();
            if (boss.attacked == true)
            {
                boss.StateChange(State.Attacked);
            }

        }
    }

    public class AttackedState : StateBase
    {
        private Boss boss;
        float waitingTime = 0;
        public AttackedState(Boss boss)
        {
            this.boss = boss;
        }
        public override void Enter()
        {
            Debug.Log("AttackedEnter");
            boss.bossHP -= 1;
            boss.anim.SetTrigger("Attacked");
            boss.OnAttacked?.Invoke();
        }
        public override void Exit()
        {
            Debug.Log("AttackedExit");
        }
        public override void Update()
        {
            if(waitingTime > 4)
            {
                boss.OnAttackedEnd?.Invoke();
                boss.StateChange(State.Idle);
                waitingTime = 0;
            }
            waitingTime += Time.deltaTime;
        }
    }

    public class Page2State : StateBase
    {
        private Boss boss;
        public Page2State(Boss boss)
        {
            this.boss = boss;
        }
        public override void Enter()
        {
            boss.OnCharged?.Invoke();
            Debug.Log("Page2Enter");
        }
        public override void Exit()
        {
            Debug.Log("Page2Exit");
        }
        public override void Update()
        {
            boss.Charge();
            if(boss.attacked == true)
            {
                boss.StateChange(State.Death);
            }
        }
    }

    public class DeathState : StateBase
    {
        private Boss boss;

        public DeathState(Boss boss)
        {
            this.boss = boss;
        }
        public override void Enter()
        {
            boss.anim.SetTrigger("");
            Debug.Log("DeathEnter");
        }
        public override void Exit()
        {
            Debug.Log("DeathExit");
        }
        public override void Update()
        {
            boss.Death();
        }
    }
}