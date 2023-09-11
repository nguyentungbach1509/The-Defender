using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool _gameOver { get; set; }

    #region Camera Boundary
    public float _minBoundHorizontal;
    public float _maxBoundHorizontal;
    public float _minBoundVertical;
    public float _maxBoundVertical;
    #endregion

    #region SpawnSystem
    [SerializeField]
    private float _interval;
    [SerializeField] GameObject _portal1;
    [SerializeField] GameObject _portal2;
    #endregion

    [SerializeField] MeowKnightControllerPC _meowKnightControllerPC;
    [SerializeField] BuildingDestroy _building;
    [SerializeField] FireKnightController _fireKnight;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    void Update()
    {
        PauseGame();
        PortalsDisable();
        if(UIManager.Instance._waveBoss)
        {
            _fireKnight.gameObject.SetActive(true);
            UIManager.Instance._waveBoss = false;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(_interval));

    }

    private IEnumerator spawnEnemy(float interval)
    {

        Vector3 spawnPosition = Random.Range(0, 2) == 0 ? _portal1.transform.position : _portal2.transform.position;
        yield return new WaitForSeconds(_interval);
        if (!UIManager.Instance._prepareWave) 
        {

            switch(UIManager.Instance._countWave)
            {
                case 2:
                    PoisonMushroom _poisonMushroom = PoolingSpawnEnemySystem.Instance.GetEnemy("PoisonMushroom").GetComponent<PoisonMushroom>();
                    if (_poisonMushroom != null)
                    {
                       
                        _poisonMushroom._health = _poisonMushroom._maxHealth; 
                        _poisonMushroom.transform.position = spawnPosition;
                        _poisonMushroom.gameObject.SetActive(true);
                    }

                    break;
                case 3:
                    BigMushroom _bigMushroom = PoolingSpawnEnemySystem.Instance.GetEnemy("BigMushroom").GetComponent<BigMushroom>();
                    if (_bigMushroom != null)
                    {
                        _bigMushroom._health = _bigMushroom._maxHealth;
                        _bigMushroom.transform.position = spawnPosition;
                        _bigMushroom.gameObject.SetActive(true);
                        _interval = 15;
                    }
                    break;
                case 4:
                    break;
                default:
                    MushroomControllerPC _baseMushroom = PoolingSpawnEnemySystem.Instance.GetEnemy("BaseMushroom").GetComponent<MushroomControllerPC>();
                    if (_baseMushroom != null) {
                        _baseMushroom.transform.position = spawnPosition;
                        _baseMushroom.gameObject.SetActive(true);
                    }
                    
                    break;
            }
        }
        StartCoroutine(spawnEnemy(interval));
 
    }

    public void PauseGame()
    {
        if(!_gameOver && (!_meowKnightControllerPC.gameObject.activeInHierarchy || _building.DestroyBuilding || _fireKnight._die))
        {
            _gameOver = true;
            UIManager.Instance.GameOverUIOpen();
            Time.timeScale = 0f;
        }
    }

    private void PortalsDisable()
    {
        if(UIManager.Instance._waveBoss)
        {
            _portal1.gameObject.SetActive(false);
            _portal2.gameObject.SetActive(false);
        }
        else
        {
            if(!_portal1.activeInHierarchy ||  !_portal2.activeInHierarchy)
            {
                _portal1.gameObject.SetActive(true);
                _portal2.gameObject.SetActive(true);
            }
            
        }

    }
}
