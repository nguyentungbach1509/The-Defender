using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    MeowKnightControllerPC _meowKnight;
    MainBuilding _mainBuilding;

    #region Boss Wave
    public bool _waveBoss { get; set; }
    public Text _warningText;
    bool _bossWaveTrigger;
    [SerializeField] float _waveBossTimeCount;
    float _waveBossTimer { get; set; }
    #endregion
    #region Timer
    public bool _prepareWave = true;
    private string _wave = "Preparing...";
    float _timer;
    [SerializeField] Text _timeText;
    public float _countWave = 1;
    #endregion

    #region TimerProgressBar
    [SerializeField] Image _mask;
    float _maximum;
    #endregion

    #region ShopUI
    [SerializeField] GameObject _shopUI;
    [SerializeField] Button[] _buttons;
    int _pointCard;
    #endregion

    #region GameOverUI
    [SerializeField] GameObject _gameOverUI;
    [SerializeField] Button _gameOverButton;
    #endregion

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        _timer = 10;
        _maximum = 10;
        _countWave = 1;
        _meowKnight = FindFirstObjectByType<MeowKnightControllerPC>();
        _mainBuilding = FindFirstObjectByType<MainBuilding>(); 
        _pointCard = 55;
    }

    // Update is called once per frame
    void Update()
    {
        CountDownWave();
    }

    public void CountDownWave()
    {
        if (_bossWaveTrigger)
        {
            if (Time.time > _waveBossTimer && !_waveBoss)
            {
                _warningText.gameObject.SetActive(false);
                _waveBoss = true;
            }
        }
        if (_timer > 0)
        {
            if(!_bossWaveTrigger) _timer -= Time.deltaTime;
            _mask.fillAmount = _timer / _maximum;
        }
        else
        {
            _mask.fillAmount = 1;
            if (_prepareWave)
            { 
                _timer = 62;
                _maximum = 62;
                if (_countWave > 3)
                {
                    _warningText.gameObject.SetActive(true);
                    _wave = "Boss Wave";
                    _waveBossTimer = Time.time + _waveBossTimeCount;
                    _bossWaveTrigger = true;
                }
                else _wave = "Wave " + _countWave;
                
                _prepareWave = false;
                _mainBuilding.ShopOpen = false;
                _shopUI.gameObject.SetActive(false);
                _meowKnight.Shopping = false;
            }
            else
            {
                _timer = 22;
                _maximum = 22;
                _wave = "Preparing...";
                if (_countWave > 1) _pointCard += 55;
                _countWave++;
                _prepareWave = true;
            }
        }
        DisplayTime(_wave, _timer);
    }

    void DisplayTime(string wave, float time)
    {
        if (time < 0) time = 0;

        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        _timeText.text = wave + " (" + string.Format("{0:00}:{1:00}", minutes, seconds) + ")";

    }

    public void ShopUIOpen()
    {
        _shopUI.gameObject.SetActive(true);
        foreach (Button button in _buttons) button.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = _pointCard + "P";
    }
    public void ShopUIClose() {_shopUI.gameObject.SetActive(false);}
    public void ShopUIPurchase()
    {
        GameObject currentCard = EventSystem.current.currentSelectedGameObject;
        Button currentButton = currentCard.GetComponent<Button>();
        currentButton.onClick.Invoke();
    }

    public void GameOverUIOpen()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_gameOverButton.gameObject);
        _gameOverUI.gameObject.SetActive(true);
        
    }

    public void GameUpUIOnClick() {
        Button currentButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        currentButton.onClick.Invoke();
    }

}
