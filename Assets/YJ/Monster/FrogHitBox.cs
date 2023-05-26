using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogHitBox : MonoBehaviour
{
    private MonsterFrog frog;

    private void Awake()
    {
        frog = GetComponentInParent<MonsterFrog>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
            frog.hp--;
    }
}
