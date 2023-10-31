using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seal : Enemy
{
    private Animator _animator;

    public override void InitAnim()
    {
        _animator = GetComponent<Animator>();
    }

    public override void AnimControl() {
        _animator.SetBool("b_Moving", true);
        if (crntState == state.Fall)
            _animator.SetBool("b_Fall", true);
    }

    public override void AnimTurn() {
        _animator.SetTrigger("t_Turn");
    }

    public override void AnimFall()
    {
        _animator.SetBool("b_Fall", true);
    }

    public override void AnimRolling() {
        _animator.SetBool("b_Rolling", true);
    }

}
