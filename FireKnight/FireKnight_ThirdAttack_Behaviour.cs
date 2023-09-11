using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireKnight_ThirdAttack_Behaviour : StateMachineBehaviour
{
    FireKnightController _fireKnight;
    Animator _animator;
    Vector3 _destinationPoint;
    Vector3 _saveStartPoint;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _fireKnight = FindFirstObjectByType<FireKnightController>();
        _animator = _fireKnight.GetComponent<Animator>();
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _fireKnight._attacking = false;
    }
}
