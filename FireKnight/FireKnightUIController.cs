using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireKnightUIController : MonoBehaviour
{
    #region FlashEffect
    SpriteRenderer _spriteRenderer;
    [SerializeField] Material _flashMaterial;
    [SerializeField] float _flashTime;
    Material _defaultMaterial;
    Coroutine _flashCoroutine;
    #endregion

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultMaterial = _spriteRenderer.material;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlashDamgeEffect()
    {
        if (_flashCoroutine != null)
        {
            StopCoroutine(FlashDamage());
        }

        _flashCoroutine = StartCoroutine(FlashDamage());
    }

    IEnumerator FlashDamage()
    {
        _spriteRenderer.material = _flashMaterial;
        yield return new WaitForSeconds(_flashTime);
        _spriteRenderer.material = _defaultMaterial;
        _flashCoroutine = null;
    }
}
