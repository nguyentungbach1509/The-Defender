using System;
using System.Collections;
using UnityEngine;


public class MeowKnightControllerPC : MonoBehaviour
{
   
    Rigidbody2D _rb;
    Animator _animator;
    public int PlayerPoints { get; set; }

    #region Movement
    public float _moveSpeed = 8f;
    float _facingDir;
    public bool Shopping { get; set; }
    #endregion

    #region Jumping
    [SerializeField]
    private LayerMask _layer;
    public float _jumpSpeed;
    [SerializeField]
    private Vector2 _boxSize;
    [SerializeField]
    private float _castDistance;
    bool _jumpPossible;

    #endregion

    #region Attack
    bool _isAttacking = false;
    [SerializeField]
    float _atkRange;
    [SerializeField]
    GameObject _atkPoint;
    [SerializeField]
    LayerMask _layerMask;
    [SerializeField] LayerMask _layerMask2;
    [SerializeField]
    float _atkDmg;
    float _nextAtk;
    [SerializeField]
    float _atkCoolDown;
    #endregion

    public float _maxHealth;
    public float _currentHealth;

    #region TakenDmg
    bool _gotStun;
    [SerializeField]
    float _invulnerableCooldown;
    [SerializeField] float _flashCooldown;
    float _flashTimer;
    #endregion

   
    #region PoisonEffect
    public bool _poisonEffect { get; set; }
    public bool _gotPoison { get; set; }
    #endregion

    #region ConfusedEffect
    public bool _confusedEffect { get; set; }
    public float _confusedDirect { get; set; }

    #endregion

    #region Burn Effect
    public bool _burnEffect { get; set; }
    public bool _gotBurn { get; set; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _confusedDirect = 1;
        PlayerPoints = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentHealth <= 0) _animator.SetBool("Die", true);
        
        if (_confusedEffect)
        {
            if (_facingDir < 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, 0);
                
            }
            if (_facingDir > 0 && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 0);
                
            }
        }
        else
        {
            if (_facingDir < 0 && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 0);
               
            }
            if (_facingDir > 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, 0);
                
            }
        }
        
        
        if(Input.GetKeyDown(KeyCode.Z) && isGrounded() && !Shopping) _jumpPossible = true;
           
        

        if(Input.GetKeyDown(KeyCode.X) && Time.time > _nextAtk && !_gotStun) StartAttack();
            
        
        _animator.SetBool("OnGround", isGrounded());
        _animator.SetFloat("VelocityY", _rb.velocity.y);
    }

    private void FixedUpdate()
    {
        if(!_gotStun)
        {
            Jump();
            Movement();
        }
    }

    private void Movement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        if (moveX != 0 && moveX != _facingDir && !_isAttacking) _facingDir = moveX;
        if (_isAttacking || Shopping) _rb.velocity = new Vector2(0, _rb.velocity.y);
        else _rb.velocity = new Vector2(moveX * _confusedDirect * _moveSpeed, _rb.velocity.y);
        _animator.SetFloat("Speed", Math.Abs(moveX));
    }

    private void Jump()
    {
        if (_jumpPossible && isGrounded())
        {
            _rb.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);

            _jumpPossible = false;
        }
    }

    public bool isGrounded()
    {   

        return Physics2D.BoxCast(transform.position, _boxSize, 0, Vector2.down, _castDistance, _layer);
    }

    private void StartAttack()
    {
        _nextAtk = Time.time + _atkCoolDown;
        _isAttacking = true;
        _animator.SetTrigger("Attack");
    }

    public void Attack()
    {
        LayerMask combinedLayerMask = _layerMask | _layerMask2;
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(_atkPoint.transform.position, _atkRange, combinedLayerMask);
        foreach (Collider2D c in collider2Ds)
        {
            if (!c.isTrigger) c.GetComponent<Enemy>().TakenDmg(_atkDmg, transform.position, true);

            
        }
    }

    public void FinishAttack()
    {
        _isAttacking = false;
        
    }

    public void TakenDamge(float dmg, EffectAttack effect)
    {
        _animator.SetTrigger("Hit");
        _currentHealth -= dmg;
        StartCoroutine(Stun());
        switch(effect)
        {
            case EffectAttack.POISON:
                 _poisonEffect = true;
                break;
            case EffectAttack.BURN:
                _burnEffect = true;
                break;
            case EffectAttack.CONFUSION:
                _confusedEffect = true;
                break;
            default: break;
            
        }
 
    }

    private IEnumerator Stun()
    {
        _gotStun = true;
        yield return new WaitForSeconds(0.5f);
        _gotStun = false;
    }

    public void GetKnockBack(Vector3 position, float _knockbackForce, float dmg)
    {
        _currentHealth -= dmg / 2;
        StartCoroutine(KnockBack(position, _knockbackForce));
    }

    private IEnumerator KnockBack(Vector3 position, float _knockbackForce)
    {
        StartCoroutine(Stun());
        Vector2 knockbackDirection = (transform.position - position).normalized;
        _rb.velocity = Vector2.zero;
        _rb.AddForce(knockbackDirection * _knockbackForce, ForceMode2D.Impulse);
        _animator.SetTrigger("Hit");
        yield return new WaitForSeconds(1.5f);
        _rb.velocity = new Vector2(0f, _rb.velocity.y);
        _gotStun = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * _castDistance, _boxSize);
        if (_atkPoint == null) return;
        Gizmos.DrawWireSphere(_atkPoint.transform.position, _atkRange);
    }

    public void Death()
    {
        gameObject.SetActive(false);
    }
}   

