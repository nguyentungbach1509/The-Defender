using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireKnight_Rolling_Behaviour : StateMachineBehaviour
{
    FireKnightController _fireKnight;
    Animator _animator;
    Rigidbody2D _rb;
    int _randomDirection;
    Vector3 _destinationPoint;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _fireKnight = FindFirstObjectByType<FireKnightController>();
        _rb = _fireKnight.GetComponent<Rigidbody2D>();
        _animator = _fireKnight.GetComponent<Animator>();
        if (_fireKnight._secondPhase) _animator.SetBool("Idle", false);
        _destinationPoint = _fireKnight.transform.localScale.x > 0 ?
            _fireKnight.transform.position + (Vector3.right * (_fireKnight._rollDistance + _fireKnight._distanceToPlayer))
            : _fireKnight.transform.position + (Vector3.left * (_fireKnight._rollDistance + _fireKnight._distanceToPlayer));

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(_fireKnight.transform.position, _destinationPoint);
        if (distance > 0.1f)
        {

            Vector3 direction = (_destinationPoint - _fireKnight.transform.position).normalized;
            Vector2 newVelocity = _fireKnight._rollSpeed * direction;
            _rb.velocity = new Vector2(newVelocity.x, 0);

        }
        else
        {
            if (!_fireKnight._secondPhase) _animator.SetBool("Idle", true);
            else _animator.SetBool("IdlePhase2", true);
            _fireKnight._isRolling = false;
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


}
