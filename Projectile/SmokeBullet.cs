using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBullet : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _maxDistance;
    [SerializeField] float _dmg;
    public Vector3 StartPoint { get; set; }
    public float MoveAngle { get; set; }
    Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Convert angle from degrees to radians
        float radians = MoveAngle * Mathf.Deg2Rad;

        // Calculate the movement direction based on the angle
        Vector2 moveDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        // Calculate the new position based on the moveDirection
        Vector3 newPosition = transform.position + (Vector3)moveDirection * _moveSpeed * Time.deltaTime;

        // Update the GameObject's position
        transform.position = newPosition;
        float distance = Vector3.Distance(transform.position, StartPoint);
        if(distance >= _maxDistance) _anim.SetTrigger("Destroy");
   
    }

    public void SmokeBulletExplode()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            MeowKnightControllerPC meow = collision.GetComponent<MeowKnightControllerPC>();
            meow.TakenDamge(_dmg, EffectAttack.CONFUSION);
            _anim.SetTrigger("Destroy");
        }
    }
}
