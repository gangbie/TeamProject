using UnityEngine;


public enum DogState
{
    Move,
    Die,
    Size
}
public class MonsterDog : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private float monsterSpeed;
    [SerializeField] private Transform[] monsterPoint;

    private MonsterBase[] monster;
    private DogState curState;
    private Animator ani;
    private SpriteRenderer render;
    private Rigidbody2D rb;
    private int monsterIndex=0;

    private void Awake()
    {
        monster = new MonsterBase[(int)DogState.Size];
        monster[(int)DogState.Move]=new MoveUpdate(this);
        monster[(int)DogState.Die]= new DieUpdate(this);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        curState=DogState.Move;
        ani = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (hp == 0)
        {
            ChangeState(DogState.Die);
        }
        monster[(int)curState].Update();
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hp--;
    }

    public void ChangeState(DogState state)
    {
        monster[(int)curState].Exit();
        curState = state;
        monster[(int)curState].Enter();
    }
    /*public void Update()
    {
        if (Vector2.Distance(transform.position, monsterPoint[monsterIndex].position)<0.02f)
        {
            monsterIndex = (monsterIndex + 1) % monsterPoint.Length;
        }
        Move();
    }
    private void Move()
    {
        
        Vector2 dir = (monsterPoint[monsterIndex].position-transform.position).normalized;
        if(dir.x<0)
            render.flipX = false;
        else if(dir.x>0)
            render.flipX = true;
        transform.Translate(dir * monsterSpeed*Time.deltaTime);
    }*/

    public class MoveUpdate : MonsterBase
    {
        private MonsterDog dog;

        public MoveUpdate(MonsterDog dog)
        {
            this.dog = dog;
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
           
        }

        public override void Update()
        {
            Vector2 dir = (dog.monsterPoint[dog.monsterIndex].position - dog.transform.position).normalized;
            if (dir.x < 0)
                dog.render.flipX = false;
            else if (dir.x > 0)
                dog.render.flipX = true;
            dog.transform.Translate(dir * dog.monsterSpeed * Time.deltaTime);
            if (Vector2.Distance(dog.transform.position, dog.monsterPoint[dog.monsterIndex].position) < 0.02f)
            {
                dog.monsterIndex = (dog.monsterIndex + 1) % dog.monsterPoint.Length;
            }
        }
    }

    public class DieUpdate : MonsterBase
    {
        private MonsterDog dog;

        public DieUpdate(MonsterDog dog)
        {
            this.dog = dog;
        }
        public override void Enter()
        {
            dog.rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            dog.rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
            dog.ani.SetBool("Dying", true);
            Destroy(dog.gameObject, t: 0.6f);
        }
    }
    
}
