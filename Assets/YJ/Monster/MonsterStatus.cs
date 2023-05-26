using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus : MonoBehaviour
{
    public int attackPoint;
    public int hp;

    public MonsterStatus(int attackPoint, int hp)
    { 
        this.attackPoint = attackPoint;
        this.hp = hp; 
    }
}
