using UnityEngine;

public class MonsterDog : MonoBehaviour
{
    [SerializeField]
    private float monsterSpeed;
    [SerializeField]
    private Transform[] monsterPoint;

    private int monsterIndex=0;
    private Rigidbody2D rb;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        Move();
    }
    private void Move()
    {
        monsterIndex= (monsterIndex+1)%monsterPoint.Length;
        Vector2 dir = (monsterPoint[monsterIndex].position-transform.position).normalized;
        transform.Translate(dir * monsterSpeed*Time.deltaTime);
    }
}
