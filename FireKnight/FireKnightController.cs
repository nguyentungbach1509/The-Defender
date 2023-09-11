using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FireKnightController : Enemy, IEnemyOnGround
{
   
    Rigidbody2D _rb;
    MeowKnightControllerPC _player;
    Animator _animator;
    
    [SerializeField] float _knockbackForce;
    [SerializeField] FireKnightUIController _UIcontroller;
    public bool _die { get; set; }

    #region Phase2
    public bool _secondPhase { get; set; }
    public bool _secondPhaseChanelling { get; set; }
    #endregion
    #region NormalAttack
    public GameObject _atkPoint;
    public GameObject _atkPointPhase2;
    public bool _attacking { get; set; }
    public float _nextAtk { get; set; }
    public float _atkPointRange;
    public float _atkPointRangePhase2;
    public float _distanceToPlayer { get; set; }
    #endregion

    #region AirAttack
    float _jumpPossible;
    float _airAttackTime;
    [SerializeField]
    private LayerMask _layer;
    public float _jumpSpeed;
    [SerializeField]
    private Vector2 _boxSize;
    [SerializeField]
    private float _castDistance;
    public bool _canJump { get; set; }
    #endregion

    #region SpecialAttack1
    [SerializeField] float _saCooldown;
    bool _availableFirstTime;
    float _sa1Next;
    public float _repeatNumber { get; set; }
    public float _distanceCasting;
    public bool _specialAtkChanneling { get; set; }
    public bool _specialAtk1State { get; set; }
    public float _specialAtk1DelayTime;
    public float _nextSpecialAtk1 { get; set; }
    #endregion

    #region DodgeRolling
    public bool _isRolling { get; set; }
    public float _rollDistance;
    public float _rollSpeed;
    #endregion

    #region Attack3
    public FireColumnSpawn _fireColumnSpawn;
    public float _fireColumnDistance;
    [SerializeField] GameObject _saveStartPosition;
    #endregion

    private void Awake()
    {
        _player =  FindFirstObjectByType<MeowKnightControllerPC>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_health <= 0) _animator.SetTrigger("Die");
        if(!_availableFirstTime)
        {
            _availableFirstTime = true;
            _sa1Next = Time.time + _saCooldown;
        }
        TurnToSecondPhase();
        if (_player != null) _distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
        if (_distanceToPlayer >= 12 && Time.time > _sa1Next && !_specialAtkChanneling && !_isRolling && !_secondPhase)
        {
            _animator.SetBool("SpecialAtk1", true);
            _sa1Next = Time.time + _saCooldown;
            _nextSpecialAtk1 = _specialAtk1DelayTime;
        }

        if (_player != null && !_player.isGrounded() && !_canJump)
        {
            _jumpPossible = Random.Range(0, 3);
            _canJump = true;
        }
        if (_player != null && _player.isGrounded()) _canJump = false;
        DirToFace();
        StartAttack();
        _animator.SetFloat("Move", Mathf.Abs(_rb.velocity.x));
        _animator.SetFloat("VelocityY", _rb.velocity.y);
        _animator.SetBool("OnGround", isGrounded());
        
    }


    private void FixedUpdate()
    {
        Jump();
        MoveOnGround();
    }

    public void MoveOnGround()
    { 
        if (isGrounded() && !_specialAtkChanneling && !_isRolling && !_secondPhaseChanelling)
        {
            Vector3 direction = (_player.transform.position - transform.position).normalized;
            if (_distanceToPlayer <= _atkRange || _attacking) _rb.velocity = new Vector2(0f, _rb.velocity.y);
            else
            {

                Vector2 newVelocity = _moveSpeed * direction;
                _rb.velocity = new Vector2(newVelocity.x, _rb.velocity.y);
            }
        }
    }

    public virtual void StartAttack()
    {
        if (_player.isGrounded() && !_specialAtkChanneling && !_isRolling)
        {
            if ((_distanceToPlayer <= _atkRange) && Time.time > _nextAtk)
            {
                int rand = Random.Range(0, 2);
                _attacking = true;
                if (rand == 0) _animator.SetBool("Attack1", true);
                else _animator.SetBool("Attack2Full", true);
                _nextAtk = Time.time + _atkCooldown;
            }
            else
            {
                if ((_distanceToPlayer <= _atkRange) && Time.time <= _nextAtk) _rb.velocity = new Vector2(0f, _rb.velocity.y);
                _animator.SetBool("Attack1", false);
                _animator.SetBool("Attack2Full", false);
            }
        }
        else
        {
            if (_distanceToPlayer <= _atkRange && _airAttackTime == 0)
            {
                _animator.SetTrigger("AirAttack");
                _nextAtk = Time.time + _atkCooldown;
                _airAttackTime = 1;
            }
        }
     
    }

    public override void NormalAttack()
    {
        if (!_secondPhase)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(_atkPoint.transform.position, _atkPointRange);
            foreach (Collider2D collider2D in collider2Ds)
            {
                if (collider2D.CompareTag("Player")) collider2D.GetComponent<MeowKnightControllerPC>().TakenDamge(_dmg, EffectAttack.NONE);
            }
        }
        else
        {
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(_atkPointPhase2.transform.position, _atkPointRangePhase2);
            foreach (Collider2D collider2D in collider2Ds)
            {
                if (collider2D.CompareTag("Player")) collider2D.GetComponent<MeowKnightControllerPC>().TakenDamge(_dmg, EffectAttack.BURN);
                
            }
        }
    }
   

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_atkPoint.transform.position, _atkPointRange);
        Gizmos.DrawWireCube(transform.position - transform.up * _castDistance, _boxSize);
        Gizmos.DrawWireSphere(_atkPointPhase2.transform.position, _atkPointRangePhase2);
    }

    public override void TakenDmg(float dmg, Vector3 position, bool stun)
    {
        if (!_isRolling)
        {
            this._health -= dmg;
            _UIcontroller.FlashDamgeEffect();
        }
        int rolling = Random.Range(0, 4);
        if (rolling == 1 && !_specialAtkChanneling)
        {
            _animator.SetTrigger("Rolling");
            _isRolling = true;
        }
    }

    private void DirToFace()
    {
        if(_player != null && !_attacking && !_specialAtk1State && !_isRolling)
        {
            if (transform.position.x < _player.transform.position.x && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 0);
            }
            if (transform.position.x > _player.transform.position.x && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 0);
            }
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _player.GetKnockBack(transform.position, _knockbackForce, _dmg);
            
        }
    }

    private void Jump()
    {
        if(isGrounded() && _jumpPossible == 2 && !_attacking && !_specialAtkChanneling && !_isRolling && !_secondPhase)
        {
            _airAttackTime = 0;
            _rb.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
            _jumpPossible = -1;
        }
 
    }

    private void TurnToSecondPhase()
    {
        if (_health <= _maxHealth / 2 && _health > 0 && !_secondPhase && !_specialAtkChanneling && !_isRolling)
        {
            _secondPhaseChanelling = true;
            _animator.SetBool("Idle", false);
            _atkPointPhase2.SetActive(true);
            _atkPoint.SetActive(false);
            _secondPhase = true;
            _animator.SetTrigger("Phase2");
            _atkRange++;
            _dmg *= 2;
            _atkCooldown = 4;
        }
    }

    public bool isGrounded()
    {
        return Physics2D.BoxCast(transform.position, _boxSize, 0, Vector2.down, _castDistance, _layer);
    }

    public void Attack3()
    {
        Vector3 destinationPoint = transform.localScale.x > 0 ?
            _saveStartPosition.transform.position + (Vector3.right * _fireColumnDistance)
            : _saveStartPosition.transform.position + (Vector3.left * _fireColumnDistance);
        _fireColumnSpawn.MoveToTarget(destinationPoint, _saveStartPosition.transform.position);
    }

    public void Die()
    {
        _die = true;
        gameObject.SetActive(false);
    }
}
