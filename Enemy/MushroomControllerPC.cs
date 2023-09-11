using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MushroomControllerPC : Enemy, IEnemyOnGround
{
    Rigidbody2D _rb;
    GameObject _gameObject;
    protected Animator _animator;
    Vector3 _targetPosition;
    protected MeowKnightControllerPC _meowKnight;
    protected float _distanceToPlayer;
    protected float _distanceToBuidling;
    Vector3 _buidlingPosition;

    #region Attack
    public GameObject _atkPoint;
    protected float _nextAtk;
    protected bool _waitingAtk;
    public float _atkPointRange;
    #endregion

    bool _gotStun;
    bool _takeDmg;
    protected bool _dealDmg;
    [SerializeField] float _knockbackForce;

    #region DeadthEffect
    [SerializeField] GameObject _canvas;
    [SerializeField] Text _givePoints;
    public bool _die { get; set; }
    #endregion


    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _gameObject = GameObject.FindGameObjectWithTag("MainBuilding");
        _meowKnight = FindFirstObjectByType<MeowKnightControllerPC>();
    }

    // Update is called once per frame
    void Update()
    {
        SelectTarget();

        if (!_gotStun) StartAttack();
        
            
        _animator.SetBool("Idle", _waitingAtk);
        _animator.SetBool("Stun", _gotStun);
    }

    void FixedUpdate()
    {
       
        if(_health > 0) MoveOnGround();
    }

    public void MoveOnGround()
    {
        
        Vector3 direction = (_targetPosition - transform.position).normalized;
        Vector3 newPosition = transform.position + _moveSpeed * Time.fixedDeltaTime * direction;
        if(!_takeDmg)
        {
            if (_distanceToPlayer <= _atkRange || _distanceToBuidling <= _atkRange || _gotStun || _dealDmg) _rb.velocity = new Vector2(0f, _rb.velocity.y);
            else _rb.MovePosition(newPosition);
        }
        

    }

    public virtual void StartAttack()
    {
        if ((_distanceToPlayer <= _atkRange || _distanceToBuidling <= _atkRange) && Time.time > _nextAtk) {
            _waitingAtk = false;
            _animator.SetBool("Attack", true);
            _nextAtk = Time.time + _atkCooldown;
        } 
        else
        {
            if ((_distanceToPlayer <= _atkRange || _distanceToBuidling <= _atkRange) && Time.time <= _nextAtk) _waitingAtk = true;
            else _waitingAtk = false;
            _animator.SetBool("Attack", false);
        }

    }

    public override void NormalAttack()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(_atkPoint.transform.position, _atkPointRange);
        foreach (Collider2D collider2D in collider2Ds)
        {
            if (collider2D.CompareTag("Player")) collider2D.GetComponent<MeowKnightControllerPC>().TakenDamge(_dmg, EffectAttack.NONE);
            if(collider2D.CompareTag("MainBuilding")) collider2D.GetComponent<MainBuilding>().TakenDamge(_dmg);
        }
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_atkPoint.transform.position, _atkPointRange);
    }

    private void SelectTarget()
    {
        if (transform.position.x < _gameObject.transform.position.x)
        {
            if (transform.localScale.x < 0) 
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, 0);
                _canvas.transform.localScale = new Vector3(Math.Abs(_canvas.transform.localScale.x), _canvas.transform.localScale.y, _canvas.transform.localScale.z);
            }
            
            _buidlingPosition = _gameObject.transform.GetChild(0).transform.position;

        }
        if (transform.position.x > _gameObject.transform.position.x)
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 0);
                _canvas.transform.localScale = new Vector3(_canvas.transform.localScale.x * -1, _canvas.transform.localScale.y, _canvas.transform.localScale.z);
            }
            _buidlingPosition = _gameObject.transform.GetChild(1).transform.position;
        }
        
        _distanceToBuidling = Vector3.Distance(transform.position, _buidlingPosition);

        if (_meowKnight != null) _distanceToPlayer = Vector3.Distance(transform.position, _meowKnight.gameObject.transform.position);

        if (_distanceToPlayer <= _distanceToBuidling && _meowKnight != null && _meowKnight.isGrounded())
        {
            _targetPosition = _meowKnight.transform.position;

            if (transform.position.x < _targetPosition.x && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, 0);
                _canvas.transform.localScale = new Vector3(Math.Abs(_canvas.transform.localScale.x), _canvas.transform.localScale.y, _canvas.transform.localScale.z);
            }
                
            if (transform.position.x > _targetPosition.x && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 0);
                _canvas.transform.localScale = new Vector3(_canvas.transform.localScale.x * -1, _canvas.transform.localScale.y, _canvas.transform.localScale.z);
            }
                
        }
        else _targetPosition = _buidlingPosition;

    }

    public override void TakenDmg(float dmg, Vector3 position, bool stun)
    {
        _takeDmg = true;
        _animator.SetTrigger("Hit_Trigger");
        this._health -= dmg;
        StartCoroutine(KnockBack(position, stun));
    }

    private IEnumerator KnockBack(Vector3 position, bool stun)
    {
        Vector2 knockbackDirection = (position - transform.position).normalized;
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _rb.AddForce(new Vector2(-knockbackDirection.x, 0f) * _knockbackForce, ForceMode2D.Impulse);
        _gotStun = stun;
        yield return new WaitForSeconds(1.5f);
        if (_health <= 0) {
            //_die = true;
            _givePoints.text = "+" + _points + "P";
            _givePoints.gameObject.SetActive(true);
            _animator.SetBool("Die", true);
        }
       
        _rb.velocity = new Vector2(0f, _rb.velocity.y);
        _takeDmg = false;
        _gotStun = false;
    }

    public void Death() {
        _meowKnight.PlayerPoints += _points;
        _givePoints.text = "";
        _health = _maxHealth;
        this.gameObject.SetActive(false);
        
    }
    public void EndDealDmg() { _dealDmg = false; }
}
