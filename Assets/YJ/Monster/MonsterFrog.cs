using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FrogState
{
    Idle,
    Jump,
    Die,
    Size
}
public class MonsterFrog : MonoBehaviour
{
    [SerializeField] private float jumpPower;
    [SerializeField] public int hp;
    [SerializeField] private LayerMask groundLayer;
    private float idleTime = 0;
    
    private MonsterBase[] monster;
    private FrogState curState;
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        monster = new MonsterBase[(int)FrogState.Size];
        monster[(int)FrogState.Idle] = new IdleUpdate(this);
        monster[(int)FrogState.Jump] = new JumpUpdate(this);
        monster[(int)FrogState.Die] = new DieUpdate(this);
    }
    private void Start()
    {
        curState = FrogState.Idle;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if(hp<=0)
        {
            FrogChangeState(FrogState.Die);
        }
        monster[(int)curState].Update();
        
    }

    public void FrogChangeState(FrogState state)
    {
        monster[(int)curState].Exit();
        curState=state;
        monster[(int)curState].Enter();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Ground")
            animator.SetBool("GroundSet", true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Ground")
            animator.SetBool("GroundSet", false);
    }
    public class IdleUpdate : MonsterBase
    {
        private MonsterFrog frog;

        public IdleUpdate(MonsterFrog forg)
        {
            this.frog = forg;
        }
        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            frog.idleTime += Time.deltaTime;
            if(frog.idleTime > 3 )
                frog.FrogChangeState(FrogState.Jump);
        }
    }
    public class JumpUpdate : MonsterBase
    {
        private MonsterFrog frog;

        public JumpUpdate(MonsterFrog frog)
        {
            this.frog = frog;
        }

        public override void Enter()
        {
            frog.idleTime = 0;
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            frog.rb.AddForce(Vector2.up * frog.jumpPower, ForceMode2D.Impulse);
            frog.FrogChangeState(FrogState.Idle);
        }
    }
    public class DieUpdate : MonsterBase
    {
        private MonsterFrog frog;

        public DieUpdate(MonsterFrog frog)
        {
            this.frog = frog;
        }
        public override void Enter()
        {
            frog.rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            frog.rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            frog.animator.SetBool("Dying", true);
            Destroy(frog.gameObject, t: 0.6f);
        }
    }

}
