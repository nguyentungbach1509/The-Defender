using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireColumn : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float _dmg;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            MeowKnightControllerPC meowKnightControllerPC = collision.GetComponent<MeowKnightControllerPC>();
            meowKnightControllerPC.TakenDamge(_dmg, EffectAttack.BURN);
        }
    }

    public void FireDisappear()
    {
        gameObject.SetActive(false);
    }
}
