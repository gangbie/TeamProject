using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerController : MonoBehaviour
{
    public UnityEvent PlayerOnTrigger;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerOnTrigger?.Invoke();
    }
}
