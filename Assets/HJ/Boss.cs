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
    private float lastJumpTime = 0;
    private float JumpCoolTime = 3;
    private StateBase[] states;
    private State curState;
    public Vector2 zero = new Vector2(0, 0);
    public Vector2 dir;
    public Animator anim;
    public UnityEvent OnAttacked;
    public UnityEvent OnAttackedEnd;
    public float waitTime;
    public int bossHP = 3;
    public bool attacked = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        states = new StateBase[(int)State.Size];
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Trace] = new TraceState(this);
        states[(int)State.Attack] = new AttackState(this);
        states[(int)State.Attacked] = new AttackedState(this);
        states[(int)State.Page2] = new Page2State(this);
        states[(int)State.Page3] = new Page3State(this);
    }

    private void Start()
    {
        curState = State.Idle;
        states[(int)curState].Enter();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        states[(int)curState].Update();
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
            if(dir.x > zero.x)
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
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        attacked = true;
    }
}

namespace BossState
{
    public enum State {Idle, Trace, Attack, Attacked, Page2, Page3, Size, Charge, Death}

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
                    boss.StateChange(State.Page2);
                    idleTime = 0;
                }
                idleTime += Time.deltaTime;
            }
            else if (boss.bossHP == 1)
            {
                if (idleTime > 3)
                {
                    boss.StateChange(State.Page3);
                    idleTime = 0;
                }
                idleTime += Time.deltaTime;
            }
        }
    }

    public class TraceState : StateBase
    {
        private Boss boss;
        private bool viewRight = false;
        private bool viewLeft = true;

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
            boss.dir = (boss.player.position - boss.transform.position);
            boss.Jump();
            if (boss.dir.x > boss.zero.x)
            {
                if (viewLeft)
                {
                    boss.transform.Rotate(Vector3.up, 180);
                    viewRight = true;
                    viewLeft = false;
                }
            }
            else
            {
                if (viewRight)
                {
                    boss.transform.Rotate(Vector3.up, 180);
                    viewRight = false;
                    viewLeft = true;
                }
            }
            if (Vector2.Distance(boss.player.position, boss.transform.position) < boss.attackRange)
            {
                boss.StateChange(State.Attack);
            }
            if (boss.attacked == true)
            {
                boss.StateChange(State.Attacked);
            }
        }
    }

    public class AttackState : StateBase
    {
        float lastAttackTime = 0;
        private Boss boss;
        public AttackState(Boss boss)
        {
            this.boss = boss;
        }
        public override void Enter()
        {
            Debug.Log("AttackEnter");
        }
        public override void Exit()
        {
            Debug.Log("AttackExit");
        }
        public override void Update()
        {
            if (lastAttackTime > 1)
            {
                boss.anim.SetTrigger("Attack");
                lastAttackTime = 0;
            }
            lastAttackTime += Time.deltaTime;
            if (Vector2.Distance(boss.player.position, boss.transform.position) > boss.attackRange)
            {
                boss.StateChange(State.Trace);
            }
        }
    }

    public class AttackedState : StateBase
    {
        private Boss boss;
        float waitingTime;
        public AttackedState(Boss boss)
        {
            this.boss = boss;
        }
        public override void Enter()
        {
            Debug.Log("AttackedEnter");
        }
        public override void Exit()
        {
            Debug.Log("AttackedExit");
        }
        public override void Update()
        {
            boss.bossHP -=  1;
            boss.anim.SetTrigger("Attacked");
            boss.OnAttacked?.Invoke();
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
        }
        public override void Exit()
        {
        }
        public override void Update()
        {
            boss.Jump();
        }
    }

    public class Page3State : StateBase
    {
        private Boss boss;
        public Page3State(Boss boss)
        {
            this.boss = boss;
        }
        public override void Enter()
        {
        }
        public override void Exit()
        {
        }
        public override void Update()
        {
            boss.Jump();
        }
    }
}