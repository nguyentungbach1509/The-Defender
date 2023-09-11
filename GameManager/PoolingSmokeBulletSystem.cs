using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingSmokeBulletSystem : MonoBehaviour
{
    public static PoolingSmokeBulletSystem Instance;
    [SerializeField] SmokeBullet _smkBullet;
    List<SmokeBullet> _smokeBulletList = new List<SmokeBullet>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 20; i++)
        {
            SmokeBullet bullet = Instantiate(_smkBullet);
            bullet.gameObject.SetActive(false);
            _smokeBulletList.Add(bullet);
        }
    }

    public SmokeBullet TakeSmokeBullet()
    {
        for(int i = 0; i < _smokeBulletList.Count; i++)
        {
            if (!_smokeBulletList[i].gameObject.activeInHierarchy) return _smokeBulletList[i];
            
        }
        return null;
    } 
}
