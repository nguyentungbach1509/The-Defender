using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class FireKnight_SpecialAtk1_Behaviour : StateMachineBehaviour
{
    FireKnightController _fireKnight;
    Rigidbody2D _rb;
    Vector3 _destinationPoint;
    Animator _animator;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _fireKnight = FindFirstObjectByType<FireKnightController>();
        _fireKnight._repeatNumber++;
        _rb = _fireKnight.GetComponent<Rigidbody2D>();
        _destinationPoint = _fireKnight.transform.localScale.x > 0 ?
            _fireKnight.transform.position + (Vector3.right * (_fireKnight._distanceCasting + _fireKnight._distanceToPlayer))
            : _fireKnight.transform.position + (Vector3.left * (_fireKnight._distanceToPlayer + _fireKnight._distanceCasting));
        _animator = _fireKnight.GetComponent<Animator>();
        _fireKnight._specialAtkChanneling = true;
        _fireKnight._specialAtk1State = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(_fireKnight.transform.position, _destinationPoint);
        if (distance > 0.1f)
        {

            Vector3 direction = (_destinationPoint - _fireKnight.transform.position).normalized;
            Vector2 newVelocity = (_fireKnight._moveSpeed * 2.5f) * direction;
            _rb.velocity = new Vector2(newVelocity.x, _rb.velocity.y);

        }
        else
        {
            _animator.SetBool("SpecialAtk1", false);
            _animator.SetBool("Idle", true);
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

   
}
