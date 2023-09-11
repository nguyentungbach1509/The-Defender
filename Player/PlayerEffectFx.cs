using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerEffectFx : MonoBehaviour
{
    public static PlayerEffectFx Instace;
    SpriteRenderer _spriteRenderer;
    [SerializeField] MeowKnightControllerPC _meowKnightControllerPC;
    [SerializeField] Image[] _statusEffects = new Image[4];
    [SerializeField] Sprite[] _statusSprites = new Sprite[4]; 
    
    int _keepTrack = 0;
    Dictionary<string, int> _indexEffect = new Dictionary<string, int>();

    #region Poison
    float _tempMoveSpeed;
    float _tempJumpSpeed;
    float _poisonFlashTime = 1f;
    float _nextPoison;
    [SerializeField] float _poisonDuration;
    float _poisonDurationCountDown;
    float _countPoison = 0;
    #endregion 

    #region Confusion
    [SerializeField] float _confusedDuration;
    [SerializeField] GameObject _confusion;
    bool _isConfusing;
    #endregion

    #region Burn
    [SerializeField] float _burnDuration;
    [SerializeField] GameObject _burn;
    float _nextBurn;
    float _burnDurationCountDown;
    float _burnFlashTime = 1f;
    float _countBurn = 0;
    #endregion

    private void Awake()
    {
        if (Instace == null) Instace = this;
        else if(Instace != this) Destroy(gameObject);
    }

    private void Start()
    {
        _tempMoveSpeed = _meowKnightControllerPC._moveSpeed;
        _tempJumpSpeed = _meowKnightControllerPC._jumpSpeed;
        _spriteRenderer = _meowKnightControllerPC.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        GotPoisonEffect();
        GotBurnEffect();
    }

    public void PoisonEffect()
    {
        if(!_meowKnightControllerPC._gotPoison)
        {
            _statusEffects[_keepTrack].sprite = _statusSprites[0];
            _statusEffects[_keepTrack].gameObject.SetActive(true);
            _poisonDurationCountDown = Time.time + _poisonDuration;
            _meowKnightControllerPC._moveSpeed -= 2;
            _meowKnightControllerPC._jumpSpeed -= 2;          
            _meowKnightControllerPC._gotPoison = true;
            _indexEffect.Add("poison", _keepTrack);
            _keepTrack++;
        }
       
    }

    private void GotPoisonEffect()
    {
        if(_meowKnightControllerPC._poisonEffect)
        {
            if (Time.time <= _poisonDurationCountDown)
            {
                if (_meowKnightControllerPC._gotPoison)
                {
                    if (Mathf.Round(Time.time) > _nextPoison)
                    {
                        _meowKnightControllerPC._currentHealth -= 1;
                        _spriteRenderer.color = new Color(0.45f, 0.03f, 0.3f, 0.75f);
                        if (_countPoison == 0)
                        {
                            _countPoison++;
                            _nextPoison = Mathf.Round(Time.time) + _poisonFlashTime;
                        }

                    }
                    else if (Mathf.Round(Time.time) == _nextPoison)
                    {
                        _spriteRenderer.color = Color.white;
                        _countPoison = 0;
                    }
                }
            }
            else ResetToNoPoison();
        }
    }

    private void ResetToNoPoison()
    {
        _spriteRenderer.color = Color.white;
        if (_meowKnightControllerPC._gotPoison) 
        { 
            _meowKnightControllerPC._poisonEffect = false;
            _statusEffects[_indexEffect["poison"]].gameObject.SetActive(false);
            _keepTrack--;
            _indexEffect.Remove("poison");
        }
        _meowKnightControllerPC._gotPoison = false;
        _meowKnightControllerPC._moveSpeed = _tempMoveSpeed;
        _meowKnightControllerPC._jumpSpeed = _tempJumpSpeed;
        _countPoison = 0;
    }

    public void ConfusionEffect()
    {
        if(!_isConfusing) StartCoroutine(GotConfusion());
    }
        

    IEnumerator GotConfusion()
    {
        _isConfusing = true;
        _confusion.gameObject.SetActive(true);
        _statusEffects[_keepTrack].sprite = _statusSprites[2];
        _statusEffects[_keepTrack].color = new Color(0.38f, 0.129f, 0.156f, 1);
        _statusEffects[_keepTrack].gameObject.SetActive(true);
        _indexEffect.Add("confusion", _keepTrack);
        _keepTrack++;
        _meowKnightControllerPC._confusedDirect = -1;
        yield return new WaitForSeconds(_confusedDuration);
        _meowKnightControllerPC._confusedDirect = 1;
        _confusion.gameObject.SetActive(false);
        _meowKnightControllerPC._confusedEffect = false;
        _statusEffects[_indexEffect["confusion"]].color = Color.white;
        _statusEffects[_indexEffect["confusion"]].gameObject.SetActive(false);
        _keepTrack--;
        _indexEffect.Remove("confusion");
        _isConfusing = false;
    }

    public void BurnEffect()
    {
        if (!_meowKnightControllerPC._gotBurn)
        {
            _statusEffects[_keepTrack].sprite = _statusSprites[1];
            _statusEffects[_keepTrack].gameObject.SetActive(true);
            _burn.gameObject.SetActive(true);
            _burnDurationCountDown = Time.time + _burnDuration;
            _meowKnightControllerPC._gotBurn = true;
            _indexEffect.Add("burn", _keepTrack);
            _keepTrack++;
        }

    }

    private void GotBurnEffect()
    {
        if (_meowKnightControllerPC._burnEffect)
        {
            if (Time.time <= _burnDurationCountDown)
            {
                if (_meowKnightControllerPC._gotBurn)
                {
                    if (Mathf.Round(Time.time) > _nextBurn)
                    {
                        _meowKnightControllerPC._currentHealth -= 2;
                        if (_countBurn == 0)
                        {
                            _countBurn++;
                            _nextBurn = Mathf.Round(Time.time) + _burnFlashTime;
                        }

                    }
                    else if (Mathf.Round(Time.time) == _nextBurn) _countBurn = 0;
                    
                }
            }
            else ResetToNoBurn();
        }
    }

    private void ResetToNoBurn()
    {
        if (_meowKnightControllerPC._gotBurn)
        {
            _meowKnightControllerPC._burnEffect = false;
            _statusEffects[_indexEffect["burn"]].gameObject.SetActive(false);
            _keepTrack--;
            _indexEffect.Remove("burn");
            _burn.gameObject.SetActive(false);
        }
        _meowKnightControllerPC._gotBurn = false;
        _countBurn = 0;
    }
}
