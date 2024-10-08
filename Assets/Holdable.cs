using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holdable : Interactable
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimator(bool set)
    {
        animator.enabled = set;
    }

}
