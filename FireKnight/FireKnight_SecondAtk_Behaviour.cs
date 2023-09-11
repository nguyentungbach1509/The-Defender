using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireKnight_SecondAtk_Behaviour : StateMachineBehaviour
{
    FireKnightController _fireKnight;
    Animator _animator;
    int random;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _fireKnight = FindFirstObjectByType<FireKnightController>();
        _animator = _fireKnight.GetComponent<Animator>();
        random = Random.Range(0, 5);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (random == 2) _animator.SetBool("SpecialAtk1", true);
        _fireKnight._attacking = false;
    }
}
