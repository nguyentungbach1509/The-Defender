using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMushroom : MushroomControllerPC
{
    string _atkType;
    #region SmokeAttack
    [SerializeField] GameObject[] _smkPoints;
    [SerializeField] SmokeBullet _bullet;
    [SerializeField] float _smkCoolDown;
    [SerializeField] float _smkRange;
    float _smkNextTime;
    #endregion


    public override void StartAttack()
    {
        if(!_meowKnight.isGrounded())
        {
            _animator.SetBool(_atkType, false);
            if (Time.time > _smkNextTime && _distanceToPlayer <= _smkRange)
            {
                _dealDmg = true;
                _waitingAtk = false;
                _animator.SetBool("SmokeAttack", true);
                _smkNextTime = Time.time + _smkCoolDown;
            }
            else if (Time.time <= _smkNextTime && _distanceToPlayer <= _smkRange)
            {
                _waitingAtk = true;
            }
            else _waitingAtk = false;
            
        }
        else
        {
            if ((_distanceToPlayer <= _atkRange || _distanceToBuidling <= _atkRange) && Time.time > _nextAtk)
            {
                int random = Random.Range(0, 2);
                _waitingAtk = false;
                if (random == 0) _atkType = "Attack";
                else _atkType = "PoisonAttack";
                _animator.SetBool(_atkType, true);
                _nextAtk = Time.time + _atkCooldown;
            }
            else
            {
                if ((_distanceToPlayer <= _atkRange || _distanceToBuidling <= _atkRange) && Time.time <= _nextAtk) _waitingAtk = true;
                else _waitingAtk = false;
                _animator.SetBool(_atkType, false);
            }

            //_smkAttack = false;
            _animator.SetBool("SmokeAttack", false);
        }
        

    }
    public override void NormalAttack()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(_atkPoint.transform.position, _atkPointRange);
        foreach (Collider2D collider2D in collider2Ds)
        {
            if (collider2D.CompareTag("Player")) collider2D.GetComponent<MeowKnightControllerPC>().TakenDamge(_dmg, _atkType == "Attack" ? EffectAttack.NONE : EffectAttack.POISON);
            if (collider2D.CompareTag("MainBuilding")) collider2D.GetComponent<MainBuilding>().TakenDamge(_dmg);
        }
    }

    public void SmokeAttack()
    {
        float degree = 0;
        if(transform.localScale.x < 0)
        {
            for (int i = 1; i < _smkPoints.Length; i++)
            {
                SmokeBullet sb = PoolingSmokeBulletSystem.Instance.TakeSmokeBullet();
                sb.transform.position = _smkPoints[i].transform.position;
                sb.StartPoint = _smkPoints[i].transform.position;
                sb.MoveAngle = degree;
                sb.gameObject.SetActive(true);
                degree += 45;
            }
        }
        else
        {
            for (int i = _smkPoints.Length-1; i > 0; i--)
            {
                SmokeBullet sb = PoolingSmokeBulletSystem.Instance.TakeSmokeBullet();
                sb.transform.position = _smkPoints[i].transform.position;
                sb.StartPoint = _smkPoints[i].transform.position;
                sb.MoveAngle = degree;
                sb.gameObject.SetActive(true);
                degree += 45;
            }
        }
       
    }
    public override void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_atkPoint.transform.position, _atkPointRange);
        Gizmos.DrawWireSphere(_smkPoints[0].transform.position, _smkRange);
    }
}
