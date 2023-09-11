using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Building : MonoBehaviour
{
    public float _maxHealth;
    public float _currentHealth;

    public abstract void TakenDamge(float dmg);
    public virtual void DestroyBuilding()
    {
        Destroy(gameObject);
    }
}
