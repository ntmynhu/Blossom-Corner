using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelAnimation : MonoBehaviour
{
    private Animator anim;
    public bool isDigging;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        Debug.Log(isDigging);
        anim.SetBool("isDigging", isDigging);
    }
}
