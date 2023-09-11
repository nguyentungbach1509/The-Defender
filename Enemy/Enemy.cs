using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public enum EffectAttack
{
    NONE,
    POISON,
    BURN,
    CONFUSION,
}

public abstract class Enemy : MonoBehaviour
{
    public float _maxHealth;
    public float _dmg;
    public float _health;
    public float _moveSpeed;
    public float _atkCooldown;
    public float _atkRange;
    public int _points;

    public abstract void NormalAttack();
    public abstract void TakenDmg(float dmg, Vector3 position, bool stun);

}
