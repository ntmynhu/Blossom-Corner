using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringAnimation : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!gameObject.activeSelf)
        {
            SetIdleAnimation();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        BaseFlower flower = collision.gameObject.GetComponent<BaseFlower>();
        if (flower != null)
        {
            animator.SetBool("IsWatering", flower.IsWatering());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SetIdleAnimation();
    }

    public void SetIdleAnimation()
    {
        animator.SetBool("IsWatering", false);
    }
}
