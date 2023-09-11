using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonMushroom : MushroomControllerPC
{
    
    public override void NormalAttack()
   {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(_atkPoint.transform.position, _atkPointRange);
        foreach (Collider2D collider2D in collider2Ds)
        {
            if (collider2D.CompareTag("Player")) collider2D.GetComponent<MeowKnightControllerPC>().TakenDamge(_dmg, EffectAttack.POISON);
            if (collider2D.CompareTag("MainBuilding")) collider2D.GetComponent<MainBuilding>().TakenDamge(_dmg);
        }
    }
}
