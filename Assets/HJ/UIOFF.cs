using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOFF : MonoBehaviour
{
    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void UIoff()
    {
        anim.SetBool("UIOFF", true);
    }
}
