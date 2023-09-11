using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireKnight_Idle_Behaviour : StateMachineBehaviour
{
    FireKnightController _fireKnight;
    Animator _animator;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _fireKnight = FindFirstObjectByType<FireKnightController>();
        _animator = _fireKnight.GetComponent<Animator>();
        _fireKnight._isRolling = false;
        _fireKnight._canJump = false;
        _fireKnight._specialAtk1State = false;
        if (_fireKnight._specialAtkChanneling) _fireKnight._nextSpecialAtk1 = Time.time + _fireKnight._specialAtk1DelayTime;
        if (_fireKnight._repeatNumber >= 3)
        {
            _fireKnight._repeatNumber = 0;
            _fireKnight._specialAtkChanneling = false;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(_fireKnight._specialAtkChanneling && !_fireKnight._secondPhase)
       {
            if (Time.time >= _fireKnight._nextSpecialAtk1) _animator.SetBool("SpecialAtk1", true);
       }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    
}
