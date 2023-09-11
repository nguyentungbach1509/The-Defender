using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _HitEffect_Behaviour : StateMachineBehaviour
{
    PlayerEffectFx _buffEffect;
    MeowKnightControllerPC _meowKnight;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _meowKnight = FindAnyObjectByType<MeowKnightControllerPC>();
        _buffEffect = FindFirstObjectByType<PlayerEffectFx>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_meowKnight._poisonEffect) _buffEffect.PoisonEffect();
        if (_meowKnight._confusedEffect) _buffEffect.ConfusionEffect();
        if(_meowKnight._burnEffect) _buffEffect.BurnEffect();   
    }

   
}
