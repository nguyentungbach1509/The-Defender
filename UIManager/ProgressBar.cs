using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    #region Timer
    public bool _prepareWave = true;
    private string _wave = "Preparing...";
    [SerializeField] float _timer;
    [SerializeField] Text _timeText;
    float _countWave = 0;
    #endregion

    #region ProgressBar
    [SerializeField] Image _mask;
    [SerializeField] Image _fill;
    [SerializeField] Color _color;
    float _maximum;
    float _minimum;
    float _current;
    #endregion

    
    // Start is called before the first frame update
    void Start()
    {
        _timer = 10;
        _maximum = 10;
        _minimum = 0;
        _current = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CountDownWave();
        UpdateProgressBar();
    }

    public void CountDownWave()
    {
        if (_timer > 0) _timer -= Time.deltaTime;
        else
        {
            if (_prepareWave)
            {
                _countWave++;
                _timer = 15;
                _maximum = 15;
                _wave = "Wave " + _countWave;
                _prepareWave = false;
            }
            else
            {
                _timer = 10;
                _maximum = 10;
                _wave = "Preparing...";
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

    void UpdateProgressBar()
    {
        float currentOffset = _current - _minimum;
        float maximumOffset = _maximum - _minimum;
        float fillAmount = currentOffset / maximumOffset;
        Debug.Log(fillAmount);
        _mask.fillAmount = fillAmount;

        _fill.color = _color;
    }
}
