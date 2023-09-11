using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingFireColumnSpawnSystem : MonoBehaviour
{
    public static PoolingFireColumnSpawnSystem Instance;
    [SerializeField] FireColumn _fireColumn;
    List<FireColumn> _fireColumnList = new List<FireColumn>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            FireColumn fireColumn = Instantiate(_fireColumn);
            fireColumn.gameObject.SetActive(false);
            _fireColumnList.Add(fireColumn);
        }
    }

    public FireColumn TakeFireColumn()
    {
        for (int i = 0; i < _fireColumnList.Count; i++)
        {
            if (!_fireColumnList[i].gameObject.activeInHierarchy) return _fireColumnList[i];

        }
        return null;
    }
}
