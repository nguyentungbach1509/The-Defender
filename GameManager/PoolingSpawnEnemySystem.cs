
using System.Collections.Generic;
using UnityEngine;

public class PoolingSpawnEnemySystem : MonoBehaviour
{
    public static PoolingSpawnEnemySystem Instance;
    [SerializeField] GameObject _baseMushroom;
    [SerializeField] GameObject _poisonMushroom;
    [SerializeField] GameObject _bigMushroom;
    Dictionary<string, List<GameObject>> _spawnList = new Dictionary<string, List<GameObject>>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if(Instance != this) Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        _spawnList.Add("BaseMushroom", new List<GameObject>());
        _spawnList.Add("PoisonMushroom", new List<GameObject>());
        _spawnList.Add("BigMushroom", new List<GameObject>());
        
        for(int i = 0; i < 100; i++)
        {
            GameObject poisonMushroom = Instantiate(_poisonMushroom);
            poisonMushroom.SetActive(false);
            _spawnList["PoisonMushroom"].Add(poisonMushroom);
            GameObject baseMushroom = Instantiate(_baseMushroom);
            baseMushroom.SetActive(false);
            _spawnList["BaseMushroom"].Add(baseMushroom);
            GameObject bigMushroom = Instantiate(_bigMushroom);
            bigMushroom.SetActive(false);
            _spawnList["BigMushroom"].Add(bigMushroom);
        }
    }

    public GameObject GetEnemy(string type)
    {
        for(int i = 0; i  < _spawnList[type].Count; i++)
        {
            if (!_spawnList[type][i].activeInHierarchy) return _spawnList[type][i];
            
        }

        return null;
    }

   
}
