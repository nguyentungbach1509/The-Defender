using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBuilding : Building
{
    [SerializeField] GameObject _destroy;
    int _duringTrigger;

    #region FlashEffect
    SpriteRenderer _spriteRenderer;
    [SerializeField] Material _flashMaterial;
    [SerializeField] float _flashTime;
    Material _defaultMaterial;
    Coroutine _flashCoroutine;
    #endregion

    #region HealthBar
    [SerializeField] Image _mask;
    [SerializeField] GameObject _healthBarCanvas;
    #endregion

    public bool ShopOpen { get; set; }
    MeowKnightControllerPC _meowKnightControllerPC;
    // Start is called before the first frame update
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultMaterial = _spriteRenderer.material;
    }

    private void Update()
    {
        if (_currentHealth <= 0)
        {
            _destroy.SetActive(true);
            _healthBarCanvas.SetActive(false);
            gameObject.SetActive(false);
        }
        _mask.fillAmount = (_currentHealth / _maxHealth);

        if (_duringTrigger > 0 && UIManager.Instance._prepareWave) ShopOpen = true;

        if (Input.GetKeyDown(KeyCode.UpArrow) && ShopOpen)
        {
            _meowKnightControllerPC.Shopping = true;
            UIManager.Instance.ShopUIOpen();
        }

        if (ShopOpen && Input.GetKeyDown(KeyCode.X))
        {
            UIManager.Instance.ShopUIClose();
            _meowKnightControllerPC.Shopping = false;
        }

        if (_meowKnightControllerPC != null && _meowKnightControllerPC.Shopping)
        {
            if(Input.GetKeyDown(KeyCode.Z)) UIManager.Instance.ShopUIPurchase();
        }
    }

    public override void TakenDamge(float dmg)
    {
        if(_flashCoroutine != null)
        {
            StopCoroutine(TakenDmgFlash(dmg));
        }

        _flashCoroutine = StartCoroutine(TakenDmgFlash(dmg));
    }

    IEnumerator TakenDmgFlash(float dmg)
    {
        _currentHealth -= dmg;
        _spriteRenderer.material = _flashMaterial;
        yield return new WaitForSeconds(_flashTime);
        _spriteRenderer.material = _defaultMaterial;
        _flashCoroutine = null; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(UIManager.Instance._prepareWave)
        {
            if(collision.CompareTag("Player"))
            {
                ShopOpen = true;
                _meowKnightControllerPC = collision.GetComponent<MeowKnightControllerPC>();
            }
        }
        if (collision.CompareTag("Player")) _duringTrigger++;
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) _duringTrigger--;
    }
}
